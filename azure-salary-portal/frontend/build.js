const fs = require('fs');
const path = require('path');

// Simple build script for Static Web App deployment
console.log('Building frontend for deployment...');

// Create output directory
const distDir = path.join(__dirname, 'dist', 'payslip-portal');
if (!fs.existsSync(path.dirname(distDir))) {
    fs.mkdirSync(path.dirname(distDir), { recursive: true });
}
if (!fs.existsSync(distDir)) {
    fs.mkdirSync(distDir, { recursive: true });
}

// Function to copy files
function copyFile(src, dest) {
    try {
        if (fs.existsSync(src)) {
            fs.copyFileSync(src, dest);
            console.log(`Copied: ${src} -> ${dest}`);
        }
    } catch (error) {
        console.log(`Could not copy ${src}:`, error.message);
    }
}

// Copy HTML files
const htmlFiles = fs.readdirSync(__dirname).filter(file => file.endsWith('.html'));
htmlFiles.forEach(file => {
    copyFile(path.join(__dirname, file), path.join(distDir, file));
});

// Copy CSS files if they exist
const cssFiles = fs.readdirSync(__dirname).filter(file => file.endsWith('.css'));
cssFiles.forEach(file => {
    copyFile(path.join(__dirname, file), path.join(distDir, file));
});

// Copy JS files if they exist
const jsFiles = fs.readdirSync(__dirname).filter(file => file.endsWith('.js') && file !== 'build.js');
jsFiles.forEach(file => {
    copyFile(path.join(__dirname, file), path.join(distDir, file));
});

console.log('Frontend build completed!');
console.log(`Output directory: ${distDir}`);