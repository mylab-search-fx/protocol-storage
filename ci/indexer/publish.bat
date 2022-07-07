echo off

IF [%1]==[] goto noparam

echo "Build image '%1' and 'latest'..."
docker build --progress plain -f ./Dockerfile -t ghcr.io/mylab-search-fx/protocol-indexer:%1 -t ghcr.io/mylab-search-fx/protocol-indexer:latest ../../search/indexer

echo "Publish image '%1' ..."
docker push ghcr.io/mylab-search-fx/protocol-indexer:%1

echo "Publish image 'latest' ..."
docker push ghcr.io/mylab-search-fx/protocol-indexer:latest

goto done

:noparam
echo "Please specify image version"
goto done

:done
echo "Done!"