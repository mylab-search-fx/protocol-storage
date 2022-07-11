echo off

IF [%1]==[] goto noparam

echo "Build INDEXER image '%1' and 'latest'..."
docker build --progress plain -f ./indexer/Dockerfile -t ghcr.io/mylab-search-fx/protocol-storage-indexer:%1 -t ghcr.io/mylab-search-fx/protocol-storage-indexer:latest ../search/indexer

echo "Build SEARCHER image '%1' and 'latest'..."
docker build --progress plain -f ./searcher/Dockerfile -t ghcr.io/mylab-search-fx/protocol-storage-searcher:%1 -t ghcr.io/mylab-search-fx/protocol-storage-searcher:latest ../search/searcher

echo "Build API image '%1' and 'latest'..."
docker build --progress plain -f ./Dockerfile -t ghcr.io/mylab-search-fx/protocol-storage-api:%1 -t ghcr.io/mylab-search-fx/protocol-storage-api:latest ../src

echo "Publish INDEXER image '%1' ..."
docker push ghcr.io/mylab-search-fx/protocol-storage-indexer:%1
echo "Publish INDEXER image 'latest' ..."
docker push ghcr.io/mylab-search-fx/protocol-storage-indexer:latest

echo "Publish SEARCHER image '%1' ..."
docker push ghcr.io/mylab-search-fx/protocol-storage-searcher:%1
echo "Publish SEARCHER image 'latest' ..."
docker push ghcr.io/mylab-search-fx/protocol-storage-searcher:latest

echo "Publish API image '%1' ..."
docker push ghcr.io/mylab-search-fx/protocol-storage-api:%1
echo "Publish API image 'latest' ..."
docker push ghcr.io/mylab-search-fx/protocol-storage-api:latest

goto done

:noparam
echo "Please specify image version"
goto done

:done
echo "Done!"