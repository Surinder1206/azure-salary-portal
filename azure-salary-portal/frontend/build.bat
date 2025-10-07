@echo off
REM Simple build script for Static Web App deployment
echo Building frontend for deployment...

REM Create output directory
if not exist "dist\payslip-portal" mkdir dist\payslip-portal

REM Copy HTML files
copy *.html dist\payslip-portal\ >nul 2>&1

REM Copy CSS files if they exist
if exist *.css copy *.css dist\payslip-portal\ >nul 2>&1

REM Copy JS files if they exist  
if exist *.js copy *.js dist\payslip-portal\ >nul 2>&1

REM Copy any assets if they exist
if exist assets xcopy assets dist\payslip-portal\assets\ /E /I /Y >nul 2>&1

echo Frontend build completed!