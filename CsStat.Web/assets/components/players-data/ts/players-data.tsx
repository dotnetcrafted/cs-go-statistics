import React, { ReactNode } from 'react';
import { Table, Avatar, Divider, Tooltip } from 'antd';
import { connect } from 'react-redux';
import { fetchPlayers, startRequest, selectPlayer } from '../../../general/ts/redux/actions';
import FilterForm, { DateValues } from './filter-form';
import { AppState, Player } from '../../../general/ts/redux/types';
import ColumnsSelector from './columns-selector';

const CELL_CSS_CLASS = 'players-data__cell';
export const COLUMN_NAMES: ColumnNames = {
    ImagePath: { dataIndex: 'ImagePath', readableName: 'ImagePath' },
    Name: { dataIndex: 'Name', readableName: 'Players Name' },
    Points: { dataIndex: 'Points', readableName: 'Points' },
    KdRatio: { dataIndex: 'KdRatio', readableName: 'K/D Ratio' },
    Kills: { dataIndex: 'Kills', readableName: 'Kills' },
    Deaths: { dataIndex: 'Deaths', readableName: 'Deaths' },
    TotalGames: { dataIndex: 'TotalGames', readableName: 'Total Games' },
    KillsPerGame: { dataIndex: 'KillsPerGame', readableName: 'Kills/Game' },
    HeadShot: { dataIndex: 'HeadShot', readableName: 'HeadShot %' },
    Assists: { dataIndex: 'Assists', readableName: 'Assists' },
    AssistsPerGame: { dataIndex: 'AssistsPerGame', readableName: 'Assists/Game' },
    DefusedBombs: { dataIndex: 'DefusedBombs', readableName: 'Defused Bombs' },
    ExplodedBombs: { dataIndex: 'ExplodedBombs', readableName: 'Exploded Bombs' },
    FriendlyKills: { dataIndex: 'FriendlyKills', readableName: 'Friendly Kills' }
};

class PlayersData extends React.Component<PlayersDataProps> {
    readonly state = {
        PlayersData: [],
        columns: [
            {
                dataIndex: COLUMN_NAMES.ImagePath.dataIndex,
                render: (_link: any, record: Player) => {
                    const content = this.getAvatar(record);
                    return this.cellWrapper(record.Id, content);
                },
                width: '5%',
                className: CELL_CSS_CLASS
            },
            {
                dataIndex: COLUMN_NAMES.Name.dataIndex,
                title: COLUMN_NAMES.Name.readableName,
                className: CELL_CSS_CLASS,
                render: (_link: any, record: Player) => {
                    return this.cellWrapper(record.Id, record.Name);
                },
                sorter: (a: Player, b: Player) => a.Name.localeCompare(b.Name)
            },
            {
                dataIndex: COLUMN_NAMES.Points.dataIndex,
                title: COLUMN_NAMES.Points.readableName,
                className: CELL_CSS_CLASS,
                render: (_link: any, record: Player) => {
                    return this.cellWrapper(record.Id, record.Points);
                },
                sorter: (a: Player, b: Player) => a.Points - b.Points
            },
            {
                dataIndex: COLUMN_NAMES.KdRatio.dataIndex,
                title: () => <Tooltip title="Kills / Deaths">{COLUMN_NAMES.KdRatio.readableName}</Tooltip>,
                className: CELL_CSS_CLASS,
                render: (_link: any, record: Player) => {
                    return this.cellWrapper(record.Id, record.KdRatio);
                },
                sorter: (a: Player, b: Player) => a.KdRatio - b.KdRatio
            },
            {
                dataIndex: COLUMN_NAMES.Kills.dataIndex,
                title: COLUMN_NAMES.Kills.readableName,
                className: CELL_CSS_CLASS,
                render: (_link: any, record: Player) => {
                    return this.cellWrapper(record.Id, record.Kills);
                },
                sorter: (a: Player, b: Player) => a.Kills - b.Kills
            },
            {
                dataIndex: COLUMN_NAMES.Deaths.dataIndex,
                title: COLUMN_NAMES.Deaths.readableName,
                className: CELL_CSS_CLASS,
                render: (_link: any, record: Player) => {
                    return this.cellWrapper(record.Id, record.Deaths);
                },
                sorter: (a: Player, b: Player) => a.Deaths - b.Deaths
            },
            {
                dataIndex: COLUMN_NAMES.TotalGames.dataIndex,
                title: COLUMN_NAMES.TotalGames.readableName,
                className: CELL_CSS_CLASS,
                render: (_link: any, record: Player) => {
                    return this.cellWrapper(record.Id, record.TotalGames);
                },
                sorter: (a: Player, b: Player) => a.TotalGames - b.TotalGames
            },
            {
                dataIndex: COLUMN_NAMES.KillsPerGame.dataIndex,
                title: COLUMN_NAMES.KillsPerGame.readableName,
                className: CELL_CSS_CLASS,
                render: (_link: any, record: Player) => {
                    return this.cellWrapper(record.Id, record.KillsPerGame);
                },
                sorter: (a: Player, b: Player) => a.KillsPerGame - b.KillsPerGame
            },
            {
                dataIndex: COLUMN_NAMES.HeadShot.dataIndex,
                title: COLUMN_NAMES.HeadShot.readableName,
                className: CELL_CSS_CLASS,
                render: (_link: any, record: Player) => {
                    return this.cellWrapper(record.Id, record.HeadShot);
                },
                sorter: (a: Player, b: Player) => a.HeadShot - b.HeadShot
            },
            {
                dataIndex: COLUMN_NAMES.Assists.dataIndex,
                title: COLUMN_NAMES.Assists.dataIndex,
                className: CELL_CSS_CLASS,
                render: (_link: any, record: Player) => {
                    return this.cellWrapper(record.Id, record.Assists);
                },
                sorter: (a: Player, b: Player) => a.Assists - b.Assists
            },
            {
                dataIndex: COLUMN_NAMES.AssistsPerGame.dataIndex,
                title: COLUMN_NAMES.AssistsPerGame.readableName,
                className: CELL_CSS_CLASS,
                render: (_link: any, record: Player) => {
                    return this.cellWrapper(record.Id, record.AssistsPerGame);
                },
                sorter: (a: Player, b: Player) => a.AssistsPerGame - b.AssistsPerGame
            },
            {
                dataIndex: COLUMN_NAMES.DefusedBombs.dataIndex,
                title: COLUMN_NAMES.DefusedBombs.readableName,
                className: CELL_CSS_CLASS,
                render: (_link: any, record: Player) => {
                    return this.cellWrapper(record.Id, record.DefusedBombs);
                },
                sorter: (a: Player, b: Player) => a.DefusedBombs - b.DefusedBombs
            },
            {
                dataIndex: COLUMN_NAMES.ExplodedBombs.dataIndex,
                title: COLUMN_NAMES.ExplodedBombs.readableName,
                className: CELL_CSS_CLASS,
                render: (_link: any, record: Player) => {
                    return this.cellWrapper(record.Id, record.ExplodedBombs);
                },
                sorter: (a: Player, b: Player) => a.ExplodedBombs - b.ExplodedBombs
            },
            {
                dataIndex: COLUMN_NAMES.FriendlyKills.dataIndex,
                title: COLUMN_NAMES.FriendlyKills.readableName,
                className: CELL_CSS_CLASS,
                render: (_link: any, record: Player) => {
                    return this.cellWrapper(record.Id, record.FriendlyKills);
                },
                sorter: (a: Player, b: Player) => a.FriendlyKills - b.FriendlyKills
            }
        ]
    };

    componentDidMount() {
        this.fetchPlayers(this.props.playersDataUrl);
    }

    private fetchPlayers(playersDataUrl: string, params?: DateValues) {
        const url = new URL(playersDataUrl, window.location.origin);
        if (params) {
            url.search = new URLSearchParams(params).toString();
        }

        this.props.startRequest();

        fetch(url.toString())
            .then((res: Response) => res.json())
            .then((data: AppState) => {
                data = typeof data === 'string' ? JSON.parse(data) : data;
                console.log(data);
                this.props.fetchPlayers(data);
            });
    }

    private getAvatar(record: Player) {
        if (record.ImagePath) {
            return <Avatar className="players-data__avatar" src={record.ImagePath} />;
        }
        return <Avatar icon="user" />;
    }

    private onRowClick(record: Player) {
        this.props.selectPlayer(record.Id);
    }

    private cellWrapper(id: string, content: ReactNode) {
        const isSelectedClass = id === this.props.SelectedPlayer ? 'is-selected' : '';
        return <div className={`players-data__cell-inner ${isSelectedClass}`}>{content}</div>;
    }

    onFormSubmit = (params: DateValues) => {
        this.fetchPlayers(this.props.playersDataUrl, params);
    };

    onCheckboxesChange = (selectedColumns: []): void => {
        console.log(selectedColumns);
    };

    render() {
        const { IsLoading, DateFrom, DateTo, Players } = this.props;
        const { columns } = this.state;

        return (
            <>
                <Divider orientation="left">Choose Dates to Filter Statistics</Divider>
                <FilterForm
                    onFormSubmit={this.onFormSubmit}
                    isLoading={IsLoading}
                    dateFrom={DateFrom}
                    dateTo={DateTo}
                />
                <Divider />
                <ColumnsSelector onCheckboxesChange={this.onCheckboxesChange} />
                <Divider />
                <Table
                    className="players-data"
                    rowClassName={() => 'players-data__row'}
                    columns={columns}
                    dataSource={Players}
                    rowKey={record => record.Id}
                    pagination={false}
                    size="middle"
                    bordered={true}
                    scroll={{ x: true }}
                    loading={IsLoading}
                    onRow={record => {
                        return {
                            onClick: () => {
                                this.onRowClick(record);
                            }
                        };
                    }}
                />
            </>
        );
    }
}
type PlayersDataProps = {
    playersDataUrl: string;
    SelectedPlayer: string;
    IsLoading: boolean;
    DateFrom: string;
    DateTo: string;
    Players: Player[];
    fetchPlayers: typeof fetchPlayers;
    startRequest: typeof startRequest;
    selectPlayer: typeof selectPlayer;
};

const mapStateToProps = (state: AppState) => {
    const SelectedPlayer = state.SelectedPlayer;
    const IsLoading = state.IsLoading;
    const DateFrom = state.DateFrom;
    const DateTo = state.DateTo;
    const Players = state.Players;
    return { SelectedPlayer, IsLoading, DateFrom, DateTo, Players };
};

export type ColumnNames = {
    [key: string]: ColumnMapping;
    ImagePath: ColumnMapping;
    Name: ColumnMapping;
    Points: ColumnMapping;
    KdRatio: ColumnMapping;
    Kills: ColumnMapping;
    Deaths: ColumnMapping;
    TotalGames: ColumnMapping;
    KillsPerGame: ColumnMapping;
    HeadShot: ColumnMapping;
    Assists: ColumnMapping;
    AssistsPerGame: ColumnMapping;
    DefusedBombs: ColumnMapping;
    ExplodedBombs: ColumnMapping;
    FriendlyKills: ColumnMapping;
};

export type ColumnMapping = {
    dataIndex: string;
    readableName: string;
};
export default connect(
    mapStateToProps,
    { fetchPlayers, startRequest, selectPlayer }
)(PlayersData);
