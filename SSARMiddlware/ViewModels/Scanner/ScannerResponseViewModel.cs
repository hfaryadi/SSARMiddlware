using System.Collections.Generic;

namespace SSARMiddlware.ViewModels.Scanner
{
    internal class ScannerResponseViewModel
    {
        public string ImageHeader { get; set; }
        public List<byte[]> Images { get; set; }
    }
}
