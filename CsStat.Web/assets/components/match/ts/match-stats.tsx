import React from 'react';
import { Table } from 'antd';
import { MatchRoundModel } from 'models';
import { getPlayerById, getTableValueByMask } from 'project/helpers';
import { columns } from './enums';

interface MatchStatsProps {
    round: MatchRoundModel | null;
}

const getDigitsCount = (number: number): number => {
    return Math.abs(number).toString().length;
};

export const MatchStats: React.FC<MatchStatsProps> = ({ round }) => {
    if (!round || !Array.isArray(round.squads)) return null;

    return (
        <div className="match-stats">
            {round.squads.map((squad) => {
                const teamCss = (
                    (squad.players[0] || {}).team || ''
                ).toLocaleLowerCase();

                const digitPlaces = {
                    kills: 0,
                    assists: 0,
                    deaths: 0,
                    kd: 0,
                    kdDiff: 0,
                    adr: 0,
                    ud: 0,
                    score: 0
                };

                squad.players.forEach((player) => {
                    digitPlaces.kills = Math.max(
                        digitPlaces.kills,
                        getDigitsCount(player.kills)
                    );
                    digitPlaces.assists = Math.max(
                        digitPlaces.assists,
                        getDigitsCount(player.assists)
                    );
                    digitPlaces.deaths = Math.max(
                        digitPlaces.deaths,
                        getDigitsCount(player.deaths)
                    );
                    digitPlaces.kd = Math.max(
                        digitPlaces.kd,
                        getDigitsCount(Math.trunc(player.kd))
                    );
                    digitPlaces.kdDiff = Math.max(
                        digitPlaces.kdDiff,
                        getDigitsCount(player.kdDiff)
                    );
                    digitPlaces.adr = Math.max(
                        digitPlaces.adr,
                        getDigitsCount(player.adr)
                    );
                    digitPlaces.ud = Math.max(
                        digitPlaces.ud,
                        getDigitsCount(player.ud)
                    );
                    digitPlaces.score = Math.max(
                        digitPlaces.score,
                        getDigitsCount(player.score)
                    );
                });

                const players = squad.players
                    .map((player) => {
                        const cmsPlayer = getPlayerById(player.id);

                        if (!cmsPlayer) return null;

                        const maskedKad = (
                            <span>
                                {getTableValueByMask(
                                    [player.kills],
                                    [-digitPlaces.kills]
                                )}
                                /
                                {getTableValueByMask(
                                    [player.assists],
                                    [digitPlaces.assists],
                                    true
                                )}
                                /
                                {getTableValueByMask(
                                    [player.deaths],
                                    [digitPlaces.deaths],
                                    true
                                )}
                            </span>
                        );
                        const splittedKd = player.kd.toFixed(2).split('.');
                        const maskedKd = getTableValueByMask(
                            [Math.trunc(player.kd)],
                            [digitPlaces.kd]
                        );

                        return {
                            ...player,
                            id: player.id,
                            rank: cmsPlayer.rang,
                            name: cmsPlayer.nickName,
                            kad: maskedKad,
                            kd: `${maskedKd}.${splittedKd[1]}`,
                            kdDiff: getTableValueByMask(
                                [player.kdDiff],
                                [-digitPlaces.kdDiff]
                            ),
                            adr: getTableValueByMask(
                                [player.adr],
                                [digitPlaces.adr]
                            ),
                            ud: getTableValueByMask(
                                [player.ud],
                                [digitPlaces.ud]
                            ),
                            score: getTableValueByMask(
                                [player.score],
                                [digitPlaces.score]
                            )
                        };
                    })
                    .filter(Boolean);

                return (
                    <div
                        className={`match-stats__team ${teamCss}`}
                        key={squad.title}
                    >
                        <Table
                            className="match-stats__table"
                            rowKey={(record) => record.id}
                            // TODO: откуда он ожидает другую модель
                            // @ts-ignore
                            dataSource={players}
                            columns={columns}
                            bordered={false}
                            pagination={false}
                            rowClassName={(record) => {
                                if (record.isDied) return 'died';

                                return '';
                            }}
                        />
                    </div>
                );
            })}
        </div>
    );
};
