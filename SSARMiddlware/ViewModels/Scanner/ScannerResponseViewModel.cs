using System.Collections.Generic;

namespace SSARMiddlware.ViewModels.Scanner
{
    internal class ScannerResponseViewModel
    {
        public ScannerResponseViewModel()
        {
            this.Images = new List<Page>();
        }
        public List<Page> Images { get; set; }
    }

    internal class Page
    {
        public string Name { get; set; }
        public string Header { get; set; }
        public byte[] Image { get; set; }
    }
}
