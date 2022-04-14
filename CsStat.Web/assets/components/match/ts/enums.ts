import { ColumnProps } from 'antd/es/table';
import { MatchPlayerModel } from 'models';

export const columns: ColumnProps<MatchPlayerModel>[] = [
    {
        title: 'Rank',
        dataIndex: 'rank',
        key: 'rank',
        align: 'center',
        width: '11%'
    },
    {
        title: 'Player',
        dataIndex: 'name',
        key: 'name',
        width: '23%'
    },
    {
        title: 'K/A/D',
        dataIndex: 'kad',
        key: 'kad',
        // align: "right",
        width: '11%'
    },
    {
        title: 'KD Diff',
        dataIndex: 'kdDiff',
        key: 'kdDiff',
        // align: "right",
        width: '11%'
    },
    {
        title: 'KD',
        dataIndex: 'kd',
        key: 'kd',
        // align: "right",
        width: '11%'
    },
    {
        title: 'ADR',
        dataIndex: 'adr',
        key: 'adr',
        // align: "right",
        width: '11%'
    },
    {
        title: 'UD',
        dataIndex: 'ud',
        key: 'ud',
        // align: "right",
        width: '11%'
    },
    {
        title: 'Score',
        dataIndex: 'score',
        key: 'score',
        // align: "right",
        width: '11%'
    }
];
