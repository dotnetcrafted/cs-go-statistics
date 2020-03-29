import { ColumnProps } from 'antd/es/table';
import { MatchPlayerModel } from 'models';

export const columns: ColumnProps<MatchPlayerModel>[] = [
    {
        title: 'Rank',
        dataIndex: 'rank',
        key: 'rank',
        align: 'center',
    },
    {
        title: 'Player',
        dataIndex: 'name',
        key: 'name'
    },
    {
        title: 'K/A/D',
        dataIndex: 'kad',
        key: 'kad',
        align: 'center',
    },
    {
        title: 'KD Diff',
        dataIndex: 'kdDiff',
        key: 'kdDiff',
        align: 'center',
    },
    {
        title: 'KD',
        dataIndex: 'kd',
        key: 'kd',
        align: 'center',
    },
    {
        title: 'ADR',
        dataIndex: 'adr',
        key: 'adr',
        align: 'center',
    },
    {
        title: 'UD',
        dataIndex: 'ud',
        key: 'ud',
        align: 'center',
    },
    {
        title: 'Score',
        dataIndex: 'score',
        key: 'score',
        align: 'center',
    },
];
