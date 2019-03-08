using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;


using iTextSharp;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace PdfPoC.ViewModel {
  public class MainViewModel : ViewModelBase {

    private string _pdfText = "";

    public string PdfText {
      get {
        return _pdfText;
      }
      set {
        if (_pdfText != value) {
          _pdfText = value;
          RaisePropertyChanged("PdfText");
        }
      }
    }

    public MainViewModel() {
      string filePath = @"C:\Users\Jeremy\Desktop\Linda Stuff\testfile.pdf";
      //PdfText = GetPageText(filePath);
    }

  }
}