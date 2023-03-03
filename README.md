# Тестовое задание для поступления в GoCloudCamp

## 1. Вопросы для разогрева

1. Опишите самую интересную задачу в программировании, которую вам приходилось решать?

Самой интересной задачей была разработка микросервисов. В прошлом году я проходила курс "Продвинутая разработка микросервисов на C#" от Ozon Route 256. Там я выполняла проекты связанные с написанием сервисов с помощью ASP.NET, взаимодействием с Postgres, Apache Kafka и Redis. 

2. Расскажите о своем самом большом факапе? Что вы предприняли для решения проблемы?

Каких-то больших провалов пока не было, но со сложными проблемами я определенно сталкивалась. Решением для них был поиск в интернете, чтение документации - повышение уровня знаний.

3. Каковы ваши ожидания от участия в буткемпе?

Прокачать текущие навыки, получить новые знания и опыт, поработать над реальными проектами.

---

## 2. Разработка музыкального плейлиста

### Часть 1. Разработка основного модуля работы с плейлистом

Требуется разработать модуль для обеспечения работы с плейлистом. Модуль должен обладать следующими возможностями:
 - Play - начинает воспроизведение
 - Pause - приостанавливает воспроизведение
 - AddSong - добавляет в конец плейлиста песню
 - Next воспроизвести след песню
 - Prev воспроизвести предыдущую песню

#### Технические требования

 - Должен быть описан четко определенный интерфейс для взаимодействия с плейлистом
 - Плейлист должен быть реализован с использованием двусвязного списка.
 - Каждая песня в плейлисте должна иметь свойство Duration.
 - Воспроизведение песни не должно блокировать методы управления.
 - Метод воспроизведения должен начать воспроизведение с длительностью, ограниченной свойством Duration песни. Воспроизведение должно эмулироваться длительной операцией.
 - Следующая песня должна воспроизводиться автоматически после окончания текущей песни.
 - Метод Pause должен приостановить текущее воспроизведение, и когда воспроизведение вызывается снова, оно должно продолжаться с момента паузы.
 - Метод AddSong должен добавить новую песню в конец списка.
 - Вызов метода Next должен начать воспроизведение следущей песни. Таким образом текущее
 - спроизведение должно быть остановлено и начато воспроизведение следущей песни 
 - Вызов метода Prev должен остановить текущее воспроизведение и начать воспроизведение предыдущей песни.
 - Реализация метода AddSong должна проводиться с учетом одновременного, конкурентного доступа.
 - Следует учитывать, что воспроизведение может быть остановлено извне 
 - Реализация методов Next/Prev должна проводиться с учетом одновременного, конкурентного доступа.
 - Примечание: Все реализации должны быть тщательно протестированы и оптимизированы с точки зрения производительности.

### Часть 2: Построение API для музыкального плейлиста

Реализовать сервис, который позволит управлять музыкальным плейлистом. Доступ к сервису должен осуществляться с помощью API, который имеет возможность выполнять CRUD операции с песнями в плейлисте, а также воспроизводить, приостанавливать, переходить к следующему и предыдущему трекам. Конфигурация может храниться в любом источнике данных, будь то файл на диске, либо база данных. Для удобства интеграции с сервисом может быть реализована клиентская библиотека.

### Технические требования

* реализация задания может быть выполнена на любом языке программирования
* сервис должен обеспечивать персистентность данных
* сервис должен поддерживать все CRUD операции 
* удалять трек допускается только если он не воспроизводится в данный момент
* API должен иметь необходимые методы для взаимодействия с плейлистом.
* API должен возвращать значимые коды ошибок и сообщения в случае ошибок.


### Будет здорово, если:
* в качестве протокола взаимодействия сервиса с клиентами будете использовать gRPC
* напишите Dockerfile и docker-compose.yml
* покроете проект unit-тестами
* сделаете тестовый пример использования написанного сервиса
