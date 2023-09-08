Write-Output "Building executable"
dotnet publish -c Release
Write-Output "Building site"
.\bin\Release\net7.0\publish\charts-site-generator.exe .\_publics\
npx tailwindcss -i .\style.css -o .\_public\style.css
Copy-Item script.js .\_public\   -Force
