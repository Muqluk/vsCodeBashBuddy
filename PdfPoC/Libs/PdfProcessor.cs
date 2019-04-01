using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using iTextSharp;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace PdfPoC.Libs {
  using System.Threading.Tasks;
  class PdfProcessor {



    private void changePagesOrder(string filename) {
      try {
        PdfReader sourcePDFReader = new PdfReader(filename);
        int n = sourcePDFReader.NumberOfPages;
        //System.out.println("no of pages in pdf files..." + n);
        int totalNoPages = n;
        int tocStartsPage = 13;
        sourcePDFReader.SelectPages(String.Format("%d-%d, 2-%d", tocStartsPage, totalNoPages - 1, tocStartsPage - 2));
        //sourcePDFReader
        PdfStamper stamper = new PdfStamper(sourcePDFReader, new FileStream(@"C:\pdftest\newPdf.pdf", FileMode.OpenOrCreate));
        //PdfStamper stamper = new PdfStamper(sourcePDFReader, new FileOutputStream(RESULT2));
        stamper.Close();
      } catch (Exception ex) {
        System.Diagnostics.Debug.Write(ex);
      }
    }
    
    private string GetPageText(string filePath) {
      var sb = new StringBuilder();
      using (PdfReader reader = new PdfReader(filePath)) {
        string prevPage = "";
        for (int page = 1; page <= reader.NumberOfPages; page++) {
          ITextExtractionStrategy its = new SimpleTextExtractionStrategy();
          var s = PdfTextExtractor.GetTextFromPage(reader, 1, its);
          if (prevPage != s) sb.Append(s);
          prevPage = s;
        }
        reader.Close();
      }
      return sb.ToString();
    }
    
    private static string GetTextAll(string filePath) {
      var sb = new StringBuilder();
      try {
        using (PdfReader reader = new PdfReader(filePath)) {
          string prevPage = "";
          for (int page = 1; page <= reader.NumberOfPages; page++) {
            ITextExtractionStrategy its = new SimpleTextExtractionStrategy();
            var s = PdfTextExtractor.GetTextFromPage(reader, page, its);
            if (prevPage != s) sb.Append(s);
            prevPage = s;
          }
          reader.Close();
        }
      } catch (Exception e) {
        throw e;
      }
      return sb.ToString();
    }
  }
}
