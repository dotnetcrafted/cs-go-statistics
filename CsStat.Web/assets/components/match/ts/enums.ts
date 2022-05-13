import { ColumnProps } from 'antd/es/table';
import { MatchPlayerModel } from 'models';

export const columns: ColumnProps<MatchPlayerModel>[] = [
    {
        title: 'Player',
        dataIndex: 'name',
        key: 'name',
        width: '35%',
        className: 'player-name'
    },
    {
        title: 'K/A/D',
        dataIndex: 'kad',
        key: 'kad',
        align: "center",
        width: '15%',
        className: 'player-kad'
    },
    {
        title: 'KD Diff',
        dataIndex: 'kdDiff',
        key: 'kdDiff',
        align: "center",
        width: '10%',
        className: 'player-stats'
    },
    {
        title: 'KD',
        dataIndex: 'kd',
        key: 'kd',
        align: "center",
        width: '10%',
        className: 'player-stats'
    },
    {
        title: 'ADR',
        dataIndex: 'adr',
        key: 'adr',
        align: "center",
        width: '10%',
        className: 'player-stats'
    },
    {
        title: 'UD',
        dataIndex: 'ud',
        key: 'ud',
        align: "center",
        width: '10%',
        className: 'player-stats'
    },
    {
        title: 'Score',
        dataIndex: 'score',
        key: 'score',
        align: "center",
        width: '10%',
        className: 'player-stats'
    }
];
