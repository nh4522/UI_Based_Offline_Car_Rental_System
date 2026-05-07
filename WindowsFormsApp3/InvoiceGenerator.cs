using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.IO;
using System.Windows.Forms;
namespace WindowsFormsApp3
{
    internal class InvoiceGenerator
    {
        public void GenerateInvoicePDF(string rentalId, string carReg, string custName, DateTime rentDate, DateTime returnDate, string rentFee, int rentalDay, decimal totalAmt)
        {
            try
            {
                // Generate a unique file name with timestamp
                string fileName = $"Invoice_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

                // Ensure no file locking issues
                if (File.Exists(filePath))
                    File.Delete(filePath);

                // Create the PDF
                using (PdfWriter writer = new PdfWriter(filePath))
                using (PdfDocument pdfDoc = new PdfDocument(writer))
                using (Document doc = new Document(pdfDoc))
                {
                    // Add content
                    doc.Add(new Paragraph("Rental Invoice").SetFontSize(20).SetTextAlignment(TextAlignment.CENTER));
                    doc.Add(new Paragraph("------------------------------------------------------------"));
                    doc.Add(new Paragraph($"Rental ID: {rentalId}"));
                    doc.Add(new Paragraph($"Car Registration: {carReg}"));
                    doc.Add(new Paragraph($"Customer Name: {custName}"));
                    doc.Add(new Paragraph($"Rental Date: {rentDate:yyyy-MM-dd}"));
                    doc.Add(new Paragraph($"Return Date: {returnDate:yyyy-MM-dd}"));
                    doc.Add(new Paragraph($"Rental Fee: {rentFee}"));
                    doc.Add(new Paragraph($"Rental Days: {rentalDay}"));
                    // doc.Add(new Paragraph($"Total: {totalAmt:C}"));
                    doc.Add(new Paragraph($"Total: ৳{totalAmt:F2}"));  // F2 formats the number to 2 decimal places
                    doc.Add(new Paragraph("------------------------------------------------------------"));
                    doc.Add(new Paragraph("Thank you for choosing our service!"));
                }

                MessageBox.Show($"Invoice generated successfully!\nFile Location: {filePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating PDF: {ex.Message}\n{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void TestPDFGeneration()
        {
            try
            {
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "TestInvoice.pdf");

                using (PdfWriter writer = new PdfWriter(filePath))
                {
                    using (PdfDocument pdfDoc = new PdfDocument(writer))
                    {
                        using (Document doc = new Document(pdfDoc))
                        {
                            doc.Add(new Paragraph("Test Invoice").SetFontSize(20).SetTextAlignment(TextAlignment.CENTER));
                            doc.Add(new Paragraph("This is a test PDF."));
                        }
                    }
                }

                MessageBox.Show($"Test PDF generated successfully at: {filePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating test PDF: {ex.Message}\n{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
