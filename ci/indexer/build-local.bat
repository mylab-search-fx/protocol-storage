@echo off

echo "Build image 'local'..."
docker build --progress plain -f ./Dockerfile -t ghcr.io/mylab-tools/propocol-storage-indexer:local ../../search/indexer

echo "Done!