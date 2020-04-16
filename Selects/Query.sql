-- Best Data
-- create view 
-- BestData
-- as
select a.Score, a.ScoreGrow, a.Dead_X, a.Dead_Y, a.Generation_number, a.Clone_number, a.SesionID
from PlayerData a inner join (select Generation_number, max(Score) as Score
    from PlayerData
    group by Generation_number) b on a.Generation_number=b.Generation_number and a.Score=b.Score;
-- Totaly best score 
select max(a.Score), SesionID
from PlayerData a inner join (select Generation_number, max(Score) as Score
    from PlayerData
    group by Generation_number) b on a.Generation_number=b.Generation_number and a.Score=b.Score;
-- Avg Data
-- create view 
-- AvgData
-- as
select avg(Score) AS AvgScore, avg(ScoreGrow) AS AvgScoreGrow, avg(Dead_X) AS AvgDead_X, avg(Dead_Y) AS AvgDead_Y
from PlayerData
group by Generation_number;

