import React from 'react';
import { Table } from 'antd';
import { MatchRound } from 'general/ts/redux/types';

interface MatchDetailsStatsProps {
    round: MatchRound | null,
}

const columns = [
    {
        title: 'Name',
        dataIndex: 'name',
        key: 'name',
    },
    {
        title: 'k/a/d',
        dataIndex: 'kad',
        key: 'kad',
    },
    {
        title: 'kdDiff',
        dataIndex: 'kdDiff',
        key: 'kdDiff',
    },
    {
        title: 'kd',
        dataIndex: 'kd',
        key: 'kd',
    },
    {
        title: 'adr',
        dataIndex: 'adr',
        key: 'adr',
    },
    {
        title: 'ud',
        dataIndex: 'ud',
        key: 'adr',
    }
];

export const MatchDetailsStats = ({ round }: MatchDetailsStatsProps) => {
    if (!round || !Array.isArray(round.squads)) return null;

    return (
        <div className="match-stats">
            {
                round.squads.map((squad) => {
                    return (
                        <div className="match-stats__col">
                            <Table
                                className="match-stats__table"
                                dataSource={squad.players}
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


                            //rowClassName={() => 'players-data__row'}
                            //columns={this.getColumns()}
                            //rowKey={(record) => record.Id}
                            //pagination={false}
                            //size="middle"
                            //bordered={true}
                            //scroll={{ x: true }}
                            //loading={IsLoading}
                            // onRow={(record) => ({
                            //     onClick: () => {
                            //         this.onRowClick(record);
                            //     }
                            // })}
                    ///>