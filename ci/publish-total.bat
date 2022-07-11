echo off

IF [%1]==[] goto noparam

echo "Build INDEXER image '%1' and 'latest'..."
docker build --progress plain -f ./indexer/Dockerfile -t ghcr.io/mylab-search-fx/protocol-indexer:%1 -t ghcr.io/mylab-search-fx/protocol-indexer:latest ../search/indexer

echo "Build SEARCHER image '%1' and 'latest'..."
docker build --progress plain -f ./searcher/Dockerfile -t ghcr.io/mylab-search-fx/protocol-searcher:%1 -t ghcr.io/mylab-search-fx/protocol-searcher:latest ../search/searcher

echo "Build STORAGE image '%1' and 'latest'..."
docker build --progress plain -f ./Dockerfile -t ghcr.io/mylab-search-fx/protocol-storage:%1 -t ghcr.io/mylab-search-fx/protocol-storage:latest ../src

echo "Publish INDEXER image '%1' ..."
docker push ghcr.io/mylab-search-fx/protocol-indexer:%1
echo "Publish INDEXER image 'latest' ..."
docker push ghcr.io/mylab-search-fx/protocol-indexer:latest

echo "Publish SEARCHER image '%1' ..."
docker push ghcr.io/mylab-search-fx/protocol-searcher:%1
echo "Publish SEARCHER image 'latest' ..."
docker push ghcr.io/mylab-search-fx/protocol-searcher:latest

echo "Publish STORAGE image '%1' ..."
docker push ghcr.io/mylab-search-fx/protocol-storage:%1
echo "Publish STORAGE image 'latest' ..."
docker push ghcr.io/mylab-search-fx/protocol-storage:latest

goto done

:noparam
echo "Please specify image version"
goto done

:done
echo "Done!"