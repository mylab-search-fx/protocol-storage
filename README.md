# MyLab.ProtocolStorage
Ознакомьтесь с последними изменениями в [журнале изменений](/CHANGELOG.md).

Docker:  [![Docker image](https://img.shields.io/static/v1?label=docker&style=flat&logo=docker&message=API&color=blue)](https://github.com/mylab-search-fx/protocol-storage/pkgs/container/protocol-storage-api) [![Docker image](https://img.shields.io/static/v1?label=docker&style=flat&logo=docker&message=indexer&color=blue)](https://github.com/mylab-search-fx/protocol-storage/pkgs/container/protocol-storage-indexer) [![Docker image](https://img.shields.io/static/v1?label=docker&style=flat&logo=docker&message=searcher&color=blue)](https://github.com/mylab-search-fx/protocol-storage/pkgs/container/protocol-storage-searcher)

Спецификация `API` : [![API specification](https://img.shields.io/badge/OAS3-v1-green)](https://app.swaggerhub.com/apis/ozzy/MyLab.ProtocolStorage/1)

Клиент: [![NuGet](https://img.shields.io/nuget/v/MyLab.ProtocolStorage.Client.svg)](https://www.nuget.org/packages/MyLab.ProtocolStorage.Client/)

## Обзор

`MyLab.ProtocolStorage` - хранилище протоколов. Обеспечивает хранение и поиск событий протоколов. 

![](./doc/mylab-protocol-storage.png)

Состав решения:

* `API` - `REST-API` для полученияд доступа к функциям хранилища;
* `indexer` - индексатор событий протокола. Разработан на базе [MyLab.Search.Indexer](https://github.com/mylab-search-fx/indexer);
* `searcher` - поисковик событий протокола. Разработан на базе [MyLab.Search.Searcher](https://github.com/mylab-search-fx/searcher).



