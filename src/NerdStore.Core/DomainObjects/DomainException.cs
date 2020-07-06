using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Core.DomainObjects
{
    public class DomainException : Exception
    {
        public DomainException()
        {

        }

        public DomainException(string mensagem) : base(mensagem)
        {

        }

        public DomainException(string mensagem, Exception innerException) : base(mensagem, innerException)
        {

        }
    }
}
