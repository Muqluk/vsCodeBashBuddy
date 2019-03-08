using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfPoC.Model {
  public class InvoiceItem {
    public int PageNumber { get; set; }
    public string Name { get; set; }                    // [0]	"DOROTHY STEVENS"	string
    public string StreetAddress { get; set; }           // [1]	"5036 SW 26TH ST"	string
    public string CityStateZip { get; set; }            // [2]	"TOPEKA, KS  66614-1418"	string
    public string InvoiceDate { get; set; }             // [4]	"4/1/2019"	string
    public string InvoiceNumber { get; set; }           // [5]	"AGR12654"	string
    public string AccountNumber { get; set; }           // [6]	"0003495"	string
    public string MasterContractNumber { get; set; }    // [7]	"0003495-01"	string
    public string Terms { get; set; }                   // [8]	"NET 30"	string
    public string SubTotal { get; set; }                // [30]	"$255.55"	string
    public string TaxApplied { get; set; }              // [31]	"$0.00"	string
    public string Total { get; set; }                   // [32]	"$255.55"	string
  }
}


/*
  [0]	"DOROTHY STEVENS"	string
  [1]	"5036 SW 26TH ST"	string
  [2]	"TOPEKA, KS  66614-1418"	string
  [3]	"MAIL TO:"	string
  [4]	"4/1/2019"	string
  [5]	"AGR12654"	string
  [6]	"0003495"	string
  [7]	"0003495-01"	string
  [8]	"NET 30"	string
  [9]	"Invoice Date:"	string
  [10]	"Invoice #:"	string
  [11]	"Account #:"	string
  [12]	"Purchase Order #:"	string
  [13]	"Master Contract #:"	string
  [14]	"Terms:"	string
  [15]	"INVOICE"	string
  [16]	"Topeka 785.266.4870"	string
  [17]	"3310 SW Topeka Blvd"	string
  [18]	"Topeka, KS 66611"	string
  [19]	"www.mcelroys.com"	string
  [20]	"THANK YOU FOR CHOOSING MCELROY'S FOR YOUR PLUMBING, HEATING & AIR CONDITIONING!"	string
  [21]	"*** SILVER PEAK PERFORMANCE MAINTENANCE PLAN ***"	string
  [22]	"FOR SERVICE FROM PERIOD  4/1/2019 To 3/31/2020"	string
  [23]	"FOR SERVICE AT THE FOLLOWING LOCATION(S):"	string
  [24]	"DOROTHY STEVENS"	string
  [25]	"5036 SW 26TH ST"	string
  [26]	"TOPEKA, KS 66614"	string
  [27]	"SUBTOTAL:"	string
  [28]	"TAX:"	string
  [29]	"TOTAL:"	string
  [30]	"$255.55"	string
  [31]	"$0.00"	string
  [32]	"$255.55"	string
  [33]	"McElroy's Inc.'s maintenance program is designed to help you reduce the cost of "	string
  [34]	"operating and maintaining your HVAC systems.  Studies have shown that our type of "	string
  [35]	"maintenance program can help you:"	string
  [36]	"  * Reduce emergency breakdown."	string
  [37]	"  * Improve performance and efficiency"	string
  [38]	"  * Extend the useful life of your equipment by 20% or more."	string
  [39]	"  * Lower energy bills."	string
  [40]	"INVOICE #: AGR12654"	string
  [41]	"ACCOUNT #: 0003495"	string
  [42]	"Please detach and submit this portion with your payment"	string
  [43]	"DOROTHY STEVENS"	string
  [44]	"5036 SW 26TH ST"	string
  [45]	"TOPEKA KS 66614-1418"	string
  [46]	"INVOICE AMOUNT"	string
  [47]	"$255.55"	string
  [48]	"AMOUNT ENCLOSED"	string
  [49]	"   $"	string
  [50]	"MAIL TO:"	string
  [51]	"MCELROY'S, INC."	string
  [52]	"PO BOX 5188"	string
  [53]	"TOPEKA, KS 66605"	string
  [54]	"BALANCE DUE AFTER 30 DAYS WILL ACCRUE 1.5% SERVICE CHARGE OR THE MAXIMUM ALLOWED BY LAW"	string
*/

