CREATE TABLE IF NOT EXISTS Received_messages (username TEXT, is_bot INTEGER, first_name TEXT, last_name TEXT, language_code TEXT, chat_id INTEGER, message_id INTEGER, message_date TEXT, chat_type TEXT, message_content BLOB);
CREATE TABLE IF NOT EXISTS Settings (logPath TEXT, connString TEXT, botToken TEXT, pwd TEXT DEFAULT 'YtcPoTIZPA0WpUdc~SMCaTjL7Kvt#ne3k*{Tb|H2Kx4t227gXy', pwdIsUsing TEXT, prsFilePath TEXT);
CREATE TABLE IF NOT EXISTS Cards (id INTEGER NOT NULL UNIQUE, universityName TEXT, programName TEXT, level TEXT, studyForm TEXT, duration TEXT, studyLang TEXT, curator TEXT, phoneNumber TEXT, email TEXT, cost TEXT, PRIMARY KEY("id")) WITHOUT ROWID;
CREATE TABLE IF NOT EXISTS Session_duration (startup_time TEXT, shutdown_time TEXT, total_uptime TEXT);
CREATE TABLE IF NOT EXISTS Universities (id INTEGER NOT NULL, universityName TEXT);
CREATE TABLE IF NOT EXISTS Directions (id INTEGER NOT NULL, directionName TEXT);
CREATE TABLE IF NOT EXISTS Programs (id INTEGER NOT NULL, programName TEXT);
CREATE TABLE IF NOT EXISTS Parsing_history (timestamp TEXT, parsingStart TEXT, parsingEnd TEXT, parsingResult INTEGER);
CREATE TABLE IF NOT EXISTS Passwords_history (timestamp TEXT, oldPassword TEXT);