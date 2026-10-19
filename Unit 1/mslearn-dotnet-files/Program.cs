using Newtonsoft.Json; 

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);   

var salesFiles = FindFiles(storesDirectory);

var salesTotal = CalculateSalesTotal(salesFiles);

File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");

CreateReport(salesFiles);

IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;
    
    // Loop over each file path in salesFiles
    foreach (var file in salesFiles)
    {      
        // Read the contents of the file
        string salesJson = File.ReadAllText(file);
    
        // Parse the contents as JSON
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);
    
        // Add the amount found in the Total field to the salesTotal variable
        salesTotal += data?.Total ?? 0;
    }
    
    return salesTotal;
}

// Modified version of CalulateSalesTotal for the report
void CreateReport(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;
    string reportDetails = $"{Environment.NewLine} Details:";

    // Loop over each file
    foreach (var file in salesFiles)
    {      
        // Read the contents of the file
        string salesJson = File.ReadAllText(file);
    
        // Parse the contents as JSON
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);
    
        // Get amount found in total field
        var dataTotal = data?.Total ?? 0;

        // Add specific format to reportDetails string
        if (dataTotal != 0)
        {
            reportDetails += $"{Environment.NewLine}  {file}: {dataTotal:C}";
        }

        // Add the amount found in the Total field to the salesTotal variable
        salesTotal += dataTotal;
    }

    // Report beginning
    string reportBeginning = 
        $"Sales Summary{Environment.NewLine}" +
        "----------------------------" +
        $"{Environment.NewLine} Total Sales: {CalculateSalesTotal(salesFiles):C}" +
        $"{Environment.NewLine}";

    // Put it all in a file
    File.WriteAllText(Path.Combine(salesTotalDir, "report.txt"), $"{reportBeginning}{reportDetails}");
}

record SalesData (double Total);