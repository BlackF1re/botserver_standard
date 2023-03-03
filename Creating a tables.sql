CREATE TABLE received_messages
(
	username	TEXT,
	is_bot	INTEGER,
	first_name	TEXT,
	last_name	TEXT,
	language_code	TEXT,
	chat_id	INTEGER,
	message_id	INTEGER,
	message_date	TEXT,
	chat_type	TEXT,
	message_content	BLOB
);

CREATE TABLE settings 
(
	logPath	TEXT,
	connString	TEXT,
	botToken	TEXT,
	pwd	TEXT,
	pwdIsUsing	TEXT
);

INSERT INTO settings VALUES 
(
'TGBot_server_log.txt', 'Data Source = appDB.db', '5969998133:AAF3nNDlYNfryOulNHKtsxlhuGo_roxXYXI', NULL, 'false'
);