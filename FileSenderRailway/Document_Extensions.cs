using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSenderRailway
{
    public static class Document_Extensions
    {
        public static Document ChangeContent(this Document document, byte[] content)
            => new Document(document.Name, content, document.Created, document.Format);

    }

    
}
