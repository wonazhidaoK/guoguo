using System;

namespace GuoGuoCommunity.API
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    class FieldNameAttribute : Attribute
    {
        public string Comment { get; set; }
        private string _fileName;
        public FieldNameAttribute(string fileName)
        {
            _fileName = fileName;
        }
    }
}