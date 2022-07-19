@echo off

echo "Build image 'local'..."
docker build --progress plain -f ./Dockerfile -t ghcr.io/mylab-search-fx/protocol-storage-indexer:local ../../search/indexer

echo "Done!