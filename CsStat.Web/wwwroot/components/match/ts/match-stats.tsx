import React from 'react';
import { Table } from 'antd';
import { MatchRoundModel } from 'models';
import { getPlayerById } from 'project/helpers';
import { columns } from './enums';

interface MatchStatsProps {
    round: MatchRoundModel | null,
}

export const MatchStats: React.FC<MatchStatsProps> = ({ round }) => {
    if (!round || !Array.isArray(round.squads)) return null;

    return (
        <div className="match-stats">
            {
                round.squads.map((squad) => {
                    const teamCss = ((squad.players[0] || {}).team || "").toLocaleLowerCase();
                    const players = squad.players.map((player) => {
                        const cmsPlayer = getPlayerById(player.id);

                        if (!cmsPlayer) return player;

                        return ({
                            ...player,
                            rank: cmsPlayer.rang,
                            name: cmsPlayer.nickName
                        });
                    });
                    return (
                        <div className={`match-stats__team ${teamCss}`} key={squad.title}>
                            <Table
                                className="match-stats__table"
                                rowKey={(record) => record.id}
                                dataSource={players}
                                columns={columns}
                                bordered={false}
                                pagination={false}
                                rowClassName = {(record) => (record.isDied ? "died" : "")}
                            />
                        </div>
                    );
                })
            }
        </div>
    );
};
