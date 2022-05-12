import { ColumnProps } from 'antd/es/table';
import { MatchPlayerModel } from 'models';

export const columns: ColumnProps<MatchPlayerModel>[] = [
    {
        title: 'Player',
        dataIndex: 'name',
        key: 'name',
        width: '35%'
    },
    {
        title: 'K/A/D',
        dataIndex: 'kad',
        key: 'kad',
        align: "center",
        width: '15%'
    },
    {
        title: 'KD Diff',
        dataIndex: 'kdDiff',
        key: 'kdDiff',
        align: "center",
        width: '10%'
    },
    {
        title: 'KD',
        dataIndex: 'kd',
        key: 'kd',
        align: "center",
        width: '10%'
    },
    {
        title: 'ADR',
        dataIndex: 'adr',
        key: 'adr',
        align: "center",
        width: '10%'
    },
    {
        title: 'UD',
        dataIndex: 'ud',
        key: 'ud',
        align: "center",
        width: '10%'
    },
    {
        title: 'Score',
        dataIndex: 'score',
        key: 'score',
        align: "center",
        width: '10%'
    }
];
