
docker exec -it ksqldb-server ksql http://localhost:8088

SET 'auto.offset.reset' = 'earliest';


POST /ksql HTTP/1.1
Accept: application/vnd.ksql.v1+json
Content-Type: application/vnd.ksql.v1+json

{
  "ksql": "CREATE STREAM my_stream (my_key BIGINT KEY,user_id VARCHAR, game_id INT, created_timestamp varchar, real_amount_bet double, real_amount_win double) with (KAFKA_TOPIC='staging_bets', KEY_FORMAT='PROTOBUF', VALUE_FORMAT='JSON', timestamp='created_timestamp', timestamp_format='dd/MM/yyyy HH:mm');",
  "streamsProperties": {
    "ksql.streams.auto.offset.reset": "earliest"
  }
}


POST /ksql HTTP/1.1
Accept: application/vnd.ksql.v1+json
Content-Type: application/vnd.ksql.v1+json

{
  "ksql": "CREATE TABLE aggregated_table WITH (kafka_topic='my_table', value_format='json') AS select timestamptostring(rowtime, 'dd/MM/yyyy') as date,game_id, user_id,sum(real_amount_bet) as total_bet_amount,sum(real_amount_win) as total_win_amount from my_stream window tumbling (size 60 SECONDS) group by timestamptostring(rowtime, 'dd/MM/yyyy'), game_id, user_id"
  "streamsProperties": {
    
  }
}






/*
test commands
CREATE STREAM my_stream ( 
	my_key BIGINT KEY,
	user_id VARCHAR, 
	game_id INT, 
	created_timestamp varchar, 
	real_amount_bet double, 
	real_amount_win double
) with (
	KAFKA_TOPIC='staging_bets', 
	KEY_FORMAT='PROTOBUF', 
	VALUE_FORMAT='JSON', 
	timestamp='created_timestamp', 
	timestamp_format='dd/MM/yyyy HH:mm'
);

select * from my_stream emit changes limit 5;

CREATE TABLE aggregated_table WITH (
	kafka_topic='my_table', 
	value_format='json'
) AS
select 	timestamptostring(rowtime, 'dd/MM/yyyy') as date,
		game_id, 
		user_id,
		sum(real_amount_bet) as total_bet_amount,
		sum(real_amount_win) as total_win_amount
from my_stream 
window tumbling (size 60 SECONDS)
group by timestamptostring(rowtime, 'dd/MM/yyyy'), game_id, user_id
--HAVING 
emit changes ;

*/