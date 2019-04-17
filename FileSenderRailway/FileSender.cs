using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ResultOf;

namespace FileSenderRailway
{
    public class FileSender
    {
        private readonly ICryptographer cryptographer;
        private readonly IRecognizer recognizer;
        private readonly Func<DateTime> now;
        private readonly ISender sender;

        public FileSender(
            ICryptographer cryptographer,
            ISender sender,
            IRecognizer recognizer,
            Func<DateTime> now)
        {
            this.cryptographer = cryptographer;
            this.sender = sender;
            this.recognizer = recognizer;
            this.now = now;
        }

        public IEnumerable<FileSendResult> SendFiles(FileContent[] files, X509Certificate certificate)
        {
            return files.Select(x => SendFile(x, certificate));
        }

        private FileSendResult SendFile(FileContent file, X509Certificate certificate)
        {
            var document = PrepareDocument(file, certificate);
            if (document.IsSuccess)
                sender.Send(document.Value);
            return new FileSendResult(file, document.Error);
        }

        private Result<Document> PrepareDocument(FileContent file, X509Certificate certificate)
        {
            var document = recognizer.Recognize(file);
            string errorMessage = null;
            if (!IsValidFormatVersion(document))
                errorMessage = $"Can't prepare file to send. This format is not provided {document.Format}";
            if (!IsValidTimestamp(document))
                errorMessage = $"Can't send. This file is too old. Data of creation: {document.Created}";
            if (errorMessage == null)
                return new Result<Document>(null, document.ChangeContent(cryptographer.Sign(document.Content, certificate)));
            return new Result<Document>(errorMessage);
        }

        private bool IsValidFormatVersion(Document doc)
        {
            return doc.Format == "4.0" || doc.Format == "3.1";
        }

        private bool IsValidTimestamp(Document doc)
        {
            var oneMonthBefore = now().AddMonths(-1);
            return doc.Created > oneMonthBefore;
        }
    }
}