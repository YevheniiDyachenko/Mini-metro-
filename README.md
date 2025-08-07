# Big Data Flow Planner - Прототип Гри

Цей документ містить інструкції для налаштування та запуску ігрового прототипу "Big Data Flow Planner", створеного за допомогою C# скриптів.

## Опис

Це прототип гри-головоломки, де гравець виступає в ролі дата-інженера. Мета — будувати конвеєри (pipelines) для обробки великих даних, з'єднуючи різні типи вузлів (джерела, трансформації, приймачі) для досягнення цілей рівня, враховуючи обмеження бюджету та часу.

## Вимоги

- **Unity 2022.3 (LTS) або новіше (включно з Unity 6)**.
- Базове розуміння інтерфейсу Unity.

---

## Покрокова Інструкція для Налаштування в Unity 6

### Крок 1: Створення нового проекту Unity

1.  Відкрийте **Unity Hub**.
2.  Натисніть **New project**.
3.  Оберіть шаблон **2D (Core)** або **2D (URP)**.
4.  Введіть назву проекту та натисніть **Create project**.

### Крок 2: Імпорт скриптів

1.  Після створення проекту, відкрийте папку проекту у вашому файловому менеджері.
2.  Скопіюйте всю папку `Scripts` з цього репозиторію до папки `Assets` вашого нового проекту Unity.
3.  Поверніться до Unity. Редактор автоматично скомпілює всі нові скрипти.

### Крок 3: Створення префабів для вузлів (Nodes)

Вам потрібно створити префаби для кожного типу вузла (`DataSource`, `DataSink`, `Filter`, `Aggregate`). Ось приклад для `DataSourceNode`:

1.  В ієрархії сцени, натисніть правою кнопкою миші -> **2D Object** -> **Sprites** -> **Circle**. Це створить `GameObject` з компонентом `SpriteRenderer`.
2.  Налаштуйте `SpriteRenderer`, обравши бажаний спрайт та колір.
3.  Натисніть **Add Component** в інспекторі та додайте:
    *   `Circle Collider 2D` (щоб на вузол можна було клікати).
    *   `DataSourceNode` (ваш скрипт).
    *   `NodeAnimator` (ваш скрипт).
4.  Перетягніть цей `GameObject` з ієрархії у вікно **Project**, щоб створити **префаб**.
5.  **Повторіть цей процес** для інших типів вузлів, додаючи відповідний скрипт (`FilterNode`, `AggregateNode`, `DataSinkNode`).

### Крок 4: Створення префабу для лінії (Connection Line)

1.  В ієрархії, створіть пустий `GameObject` і назвіть його `LinePrefab`.
2.  Додайте до нього компонент **Line Renderer**. Налаштуйте його вигляд (ширину, матеріал). Можна використати вбудований `Sprites-Default` матеріал.
3.  Додайте до `LinePrefab` скрипт **DataFlowAnimator**.
4.  Створіть дочірній об'єкт для `LinePrefab` (натисніть правою кнопкою на `LinePrefab` -> **Create Empty**). Назвіть його `Particle`. Цей об'єкт буде рухатись вздовж лінії.
5.  Збережіть `LinePrefab` як префаб, перетягнувши його у вікно **Project**.

### Крок 5: Налаштування Сцени

1.  Створіть у сцені пустий `GameObject` та назвіть його `GameController`.
2.  Додайте на `GameController` наступні скрипти:
    *   `PipelineManager`
    *   `LevelManager`
    *   `PlayerInputController`
    *   `EventController`
3.  Створіть ще один пустий `GameObject` та назвіть його `Visuals`.
4.  Додайте на `Visuals` наступні скрипти:
    *   `PipelineVisualizer`
    *   `LineDrawer`

### Крок 6: Створення даних рівня (LevelData Asset)

1.  У вікні **Project**, натисніть правою кнопкою миші -> **Create** -> **Big Data Flow** -> **Level Data**. Це створить новий ассет даних.
2.  Назвіть його `Level_01_Data`.
3.  Натисніть на цей ассет, щоб відкрити його в інспекторі.
4.  Налаштуйте параметри рівня:
    *   Встановіть `Time Limit`, `Target Flow`, `Initial Budget`.
    *   У списку `Initial Node Placements`, встановіть розмір (наприклад, 3).
    *   Для кожного елемента списку:
        *   Перетягніть відповідний **префаб вузла** у поле `Node Prefab`.
        *   Встановіть `Position` та унікальний `Node Id`.

### Крок 7: Підключення Залежностей (Dependencies)

Тепер потрібно з'єднати всі компоненти в інспекторі.

1.  Оберіть `GameController` у сцені.
    *   У компоненті `Level Manager`, перетягніть ваш ассет `Level_01_Data` у поле `Current Level Data`.
    *   Перетягніть `GameObject` `GameController` (на якому є `PipelineManager`) у поле `Pipeline Manager`.
    *   У компоненті `Player Input Controller`, перетягніть `GameController` у поле `Pipeline Manager` та `Visuals` (на якому є `LineDrawer`) у поле `Line Drawer`.
2.  Оберіть `Visuals` у сцені.
    *   У компоненті `Pipeline Visualizer`, перетягніть ваш **префаб лінії** у поле `Line Prefab`.

### Крок 8: Запуск гри

Натисніть кнопку **Play** у редакторі Unity. Якщо все налаштовано правильно, на сцені з'являться початкові вузли, і ви зможете з'єднувати їх, перетягуючи мишкою.

---

## Огляд Скриптів

-   **Core Logic**: `NodeBase`, `NodeModule`, `DataSourceNode`, `DataSinkNode`, `FilterNode`, `AggregateNode`.
-   **Managers**: `PipelineManager`, `LevelManager`, `EventController`.
-   **Player Input**: `PlayerInputController`.
-   **Visuals**: `PipelineVisualizer`, `LineDrawer`, `NodeAnimator`, `DataFlowAnimator`.
-   **Data**: `LevelData`.

## Наступні Кроки

-   Розширити логіку `EventController` для генерації випадкових подій.
-   Додати більше типів вузлів (Join, Split).
-   Створити UI для відображення перемоги/поразки.
-   Розробити візуальне налаштування параметрів вузлів (наприклад, відсоток фільтрації).
