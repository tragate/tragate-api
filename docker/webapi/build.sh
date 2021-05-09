#!/bin/bash

# Define image name and version
IMAGE_NAME=ninjafx/tragate-api
VERSION=1.0


# run build script for dockerfile
projectList=(
    "../../src/Tragate.WebApi"
)

for project in "${projectList[@]}"
do
    echo -e "\e[33mWorking on $(pwd)/$project"
    echo -e "\e[33m\tRemoving old publish output"
    pushd $(pwd)/$project
    rm -rf obj/Docker/publish
    echo -e "\e[33m\tBuilding and publishing projects"
    
    echo -e "=======> Starting restore"
    dotnet restore
    
    echo -e "=======> Starting build"
    dotnet build
    
    echo -e "=======> Starting test"
    dotnet test ../../src/Tragate.Tests/Tragate.Tests.csproj
    
    rc=$?
    if [[ $rc != 0 ]] ; then
        echo "Failed to process data"
        exit $rc
    fi
    
    echo -e "=======> Starting publish"
    dotnet publish -o obj/Docker/publish -c Release
    popd
done

# Create and Copy latest built dll file into docker folder
mkdir app
mv ../../src/Tragate.WebApi/obj/Docker/publish app/
rm -rf ../../src/Tragate.WebApi/obj/Docker

# Build docker image
docker build -f Dockerfile -t "$IMAGE_NAME:$VERSION" .

# Tag this version as latest
docker tag "$IMAGE_NAME:$VERSION" "$IMAGE_NAME:latest"

# Push docker image as latest
#docker push "$IMAGE_NAME:latest"

# Remove built jar file
rm -rf app