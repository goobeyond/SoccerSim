# SoccerSim
## Introduction
This project is meant to do 2 things:
1. Simulate matches between 2 teams in a group.
2. Rank order them based on their match results.

## Setup
For the sake of simplicity, the teams, group and matches are hardcoded into the application. On startup, the data is seeded into the database.

The application is written with dotnet 6 and uses a SQLite database to store data. 

## Group
There's only one group and the ID is 1.

## Team
A team is broken down into 3 parameters: Attack, Defence and Midfield.
The attack and defence metrics are used to determine the chance of scoring a goal for that team. The midfield stat determines the number of goal attempts a team makes.

The teams are as follows:

|Name          | Att       | Mid        | Def       |
|--------------|-----------|------------|-----------|
| A            | 10        | 4          | 5         |
| B            | 5         | 6          | 3         |
| C            | 1         | 2          | 9         |
| D            | 7         | 10         | 6         |

## Simulation
For each match, the simulation is run twice, pitting the teams against each other in attacking and defending positions respectively. A score is calculated for each run.

The formula used to simulate each attack is as follows:

`chance to score = (10 + attacker.Att - defender.Def) / 2`

The largest possible gap between an attack stat and defend stat is in the range of 10 to -10. That gives us a range of 20. Therefore we add 10 to the difference between those stats to keep the value in the positive. Then we would divide that number by 20 to calculate the chance to score. we'd then have to multiply that value by 10 again the convert it to an integer, so to simplify, we just divide by 2.

For example, if we pit Team B against Team C, for team B, the chance to score would be
- `(10 + 5 - 9) / 2 = 3` or in other words, 30 percent.

After each game is simulated, the match results are updated and team standings are updated, but not ordered.

A game between 2 teams in a group can only be simulated once. But a separate endpoint exists so that a match between 2 teams can be repeated as many times as desired, but the results are not stored anywhere.

## Standings
The standings are calculated everytime the `GET group/{groupId}/standings` endpoint is called. The endpoint takes advantage of the `RANK()` command in SQL to generate a rank number based on the first 4 criteria defined by the assignment (Points, Goal difference, Goals for, Goals against).

If two teams end up with the same ranking, we then look at their head to head match result and lower the rank of the loser.

Should those two teams have a draw or have not played their match yet, we randomly lower the rank of one of them.

## Limitations and Improvements
First and foremost, this application is missing any sort of team and group management. Ideally, we could have dynamic number of teams in groups and the matches that had to be played would also be dynamically calculated.

Secondly, we could take advantage of caching to prevent calculating the standings on every API call. We could invalidate the cache after a game is played in the group.
