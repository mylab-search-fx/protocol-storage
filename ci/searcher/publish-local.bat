@echo off

echo "Build image 'local'..."
docker build --progress plain -f ./Dockerfile -t ghcr.io/mylab-search-fx/protocol-searcher:local ../../search/searcher

echo "Done!