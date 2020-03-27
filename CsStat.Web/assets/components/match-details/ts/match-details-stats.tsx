import React from 'react';
import { Table } from 'antd';
import { getPlayerById } from 'project/helpers';
import { MatchRound } from 'general/ts/redux/types';

interface MatchDetailsStatsProps {
    round: MatchRound | null,
}

const columns = [
    {
        title: 'Rank',
        dataIndex: 'rank',
        key: 'rank',
    },
    {
        title: 'Player',
        dataIndex: 'name',
        key: 'name',
    },
    {
        title: 'K/A/D',
        dataIndex: 'kad',
        key: 'kad',
    },
    {
        title: 'KD Diff',
        dataIndex: 'kdDiff',
        key: 'kdDiff',
    },
    {
        title: 'KD',
        dataIndex: 'kd',
        key: 'kd',
    },
    {
        title: 'ADR',
        dataIndex: 'adr',
        key: 'adr',
    },
    {
        title: 'UD',
        dataIndex: 'ud',
        key: 'ud',
    }
];

export const MatchDetailsStats = ({ round }: MatchDetailsStatsProps) => {
    if (!round || !Array.isArray(round.squads)) return null;


    return (
        <div className="match-stats">
            {
                round.squads.map((squad) => {
                    const players = squad.players.map((player) => {
                        const cmsPlayer = getPlayerById(player.id);

                        if (!cmsPlayer) return player;

                        return ({
                            ...player,
                            rank: cmsPlayer.rang,
                            name: cmsPlayer.nickName
                        })
                    })
                    return (
                        <div className="match-stats__team" key={squad.title}>
                            <h3 className="match-stats__title">{squad.title}</h3>
                            <Table
                                className="match-stats__table"
                                rowKey={(record) => record.id}
                                dataSource={players}
                                columns={columns}
                                bordered={true}
                                pagination={false}
                            />
                        </div>
                    );
                })
            }
        </div>
    );
}
