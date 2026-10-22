// Reads the image and the connection string from the environment so neither
// lands on disk. The first deployment runs a public placeholder image
// (mcr.microsoft.com/dotnet/samples:aspnetapp) until GitHub Actions deploys the
// API; after that pass the live image (README, "Deploying the API").

using './api.bicep'

param image = readEnvironmentVariable('API_IMAGE')
param dbConnectionString = readEnvironmentVariable('API_DB_CONNECTION')
