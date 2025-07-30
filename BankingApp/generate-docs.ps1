# Create HTML header
$html = @"
<!DOCTYPE html>
<html>
<head>
  <title>BankingApp Full Documentation</title>
  <style>
    body { font-family: sans-serif; margin: 2em; line-height: 1.6 }
    .type { margin-bottom: 3em; border-bottom: 1px solid #eee; padding-bottom: 1.5em }
    h2 { color: #2c3e50; border-bottom: 2px solid #3498db; padding-bottom: 0.3em }
    .remarks { background: #f8f9fa; padding: 1em; border-left: 4px solid #3498db; margin: 1em 0 }
    .params { margin: 1em 0 }
    .param { margin-bottom: 0.5em }
    .param-name { font-weight: bold; color: #2980b9 }
    .returns { font-style: italic; margin-top: 1em }
    .exception { color: #e74c3c }
    .list-type-bullet { list-style-type: disc; padding-left: 2em }
  </style>
</head>
<body>
  <h1>BankingApp Complete Documentation</h1>
"@

# Process XML
$xmlPath = "bin\Debug\net9.0\BankingApp.xml"
$xml = [xml](Get-Content $xmlPath)

# Process each type
$xml.doc.members.member | Where-Object { $_.name -like 'T:*' } | ForEach-Object {
    $typeName = $_.name.Substring(2)
    $summary = $_.summary
    $remarks = $_.remarks
    
    $html += "<div class='type'><h2>$typeName</h2>"
    $html += "<div class='summary'>$summary</div>"
    
    if ($remarks) {
        $html += "<div class='remarks'><h3>Remarks</h3>$remarks</div>"
    }
    
    # Get all methods for this type
    $methods = $xml.doc.members.member | Where-Object { $_.name -like "M:$typeName*" }
    if ($methods) {
        $html += "<h3>Methods</h3>"
        foreach ($method in $methods) {
            $methodName = $method.name.Substring($method.name.IndexOf(":") + 1)
            $html += "<div class='method'><h4>$methodName</h4>"
            
            if ($method.summary) {
                $html += "<div class='summary'>$($method.summary)</div>"
            }
            
            # Parameters
            if ($method.param) {
                $html += "<div class='params'><h5>Parameters</h5>"
                foreach ($param in $method.param) {
                    $html += "<div class='param'><span class='param-name'>$($param.name)</span>: $($param.'#text')</div>"
                }
                $html += "</div>"
            }
            
            # Returns
            if ($method.returns) {
                $html += "<div class='returns'><strong>Returns:</strong> $($method.returns)</div>"
            }
            
            # Exceptions
            if ($method.exception) {
                $html += "<div class='exceptions'><h5>Exceptions</h5>"
                foreach ($exception in $method.exception) {
                    $html += "<div class='exception'><strong>$($exception.cref.Substring(2))</strong>: $($exception.'#text')</div>"
                }
                $html += "</div>"
            }
            
            $html += "</div>"
        }
    }
    
    $html += "</div>"
}

$html += "</body></html>"
$html | Out-File "docs.html"

Write-Host "Complete documentation generated at docs.html"
Start-Process "docs.html"