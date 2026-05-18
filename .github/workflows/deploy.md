name: Deploy to GitHub Pages

on:
  push:
    branches: [ main ]

permissions:
  contents: write
  pages: write
  actions: read

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Setup .NET 10
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.0.x'
          include-prerelease: true

      - name: Verify .NET version
        run: dotnet --version

      - name: Publish Project
        run: dotnet publish apps/KwanzaSmart.Client/KwanzaSmart.Client.csproj -c Release -o release

      - name: Add .nojekyll
        run: touch release/wwwroot/.nojekyll

      - name: Copy index.html to 404.html
        run: cp release/wwwroot/index.html release/wwwroot/404.html

      - name: Deploy to GitHub Pages
        uses: JamesIves/github-pages-deploy-action@v4
        with:
          folder: release/wwwroot
          branch: gh-pages
          token: ${{ secrets.GITHUB_TOKEN }}