import React, { ReactNode } from 'react';
import {
    Table, Avatar, Divider, Tooltip, Dropdown, Icon, Button
} from 'antd';
import { connect } from 'react-redux';
import { ColumnProps } from 'antd/es/table';
import qs from 'query-string';
import {
    fetchPlayers, startRequest, stopRequest
} from '../../../general/ts/redux/actions';
import FilterForm, { DateValues } from './filter-form';
import { IAppState, RootState, Player } from '../../../general/ts/redux/types';
import ColumnsSelector from './columns-selector';
import { nameof } from '../../../general/ts/extentions';
import '../scss/index.scss';
import utils from '../../../general/ts/utils';
import { history } from '../../../general/ts/redux/store';

const CELL_CSS_CLASS = 'players-data__cell';
const HIDDEN_CELL_CSS_CLASS = 'is-hidden';

export const COLUMN_NAMES: ColumnNames = {
    ImagePath: { dataIndex: nameof<Player>('ImagePath'), readableName: 'ImagePath' },
    Name: { dataIndex: nameof<Player>('Name'), readableName: 'Players Name' },
    Points: { dataIndex: nameof<Player>('Points'), readableName: 'Points' },
    Kad: { dataIndex: nameof<Player>('Kad'), readableName: 'K/A/D' },
    KdRatio: { dataIndex: nameof<Player>('KdRatio'), readableName: 'KD Ratio' },
    KdDif: { dataIndex: nameof<Player>('KdDif'), readableName: 'KD Diff' },
    HeadShot: { dataIndex: nameof<Player>('HeadShot'), readableName: 'HeadShots' },
    TotalGames: { dataIndex: nameof<Player>('TotalGames'), readableName: 'Total Games' },
    Kills: { dataIndex: nameof<Player>('Kills'), readableName: 'Kills' },
    Assists: { dataIndex: nameof<Player>('Assists'), readableName: 'Assists' },
    Deaths: { dataIndex: nameof<Player>('Deaths'), readableName: 'Deaths' },
    KillsPerGame: { dataIndex: nameof<Player>('KillsPerGame'), readableName: 'Kills/Game' },
    AssistsPerGame: { dataIndex: nameof<Player>('AssistsPerGame'), readableName: 'Assists/Game' },
    DefusedBombs: { dataIndex: nameof<Player>('DefusedBombs'), readableName: 'Defused Bombs' },
    ExplodedBombs: { dataIndex: nameof<Player>('ExplodedBombs'), readableName: 'Exploded Bombs' },
    FriendlyKills: { dataIndex: nameof<Player>('FriendlyKills'), readableName: 'Friendly Kills' },
};
const DEFAULT_COLUMNS = [
    COLUMN_NAMES.Points.dataIndex,
    COLUMN_NAMES.Kad.dataIndex,
    COLUMN_NAMES.KdRatio.dataIndex,
    COLUMN_NAMES.KdDif.dataIndex,
    COLUMN_NAMES.HeadShot.dataIndex,
    COLUMN_NAMES.TotalGames.dataIndex,
];

const PERMANENT_COLUMNS = [COLUMN_NAMES.ImagePath.dataIndex, COLUMN_NAMES.Name.dataIndex];
class PlayersData extends React.Component<PlayersDataProps, PlayersDataState> {
    readonly state = {
        visibleColumns: [...DEFAULT_COLUMNS, ...PERMANENT_COLUMNS]
    };

    private fetchPlayers(playersDataUrl: string, params?: DateValues): void {
        const url = new URL(playersDataUrl, window.location.origin);
        if (params) {
            url.search = new URLSearchParams(params).toString();
        }

        this.props.startRequest();

        fetch(url.toString())
            .then((res: Response) => res.json())
            .then((data: IAppState) => {
                data = typeof data === 'string' ? JSON.parse(data) : data;
                this.props.fetchPlayers(data);
            })
            .catch((error) => {
                this.props.stopRequest();
                throw new Error(error);
            });
    }

    private getAvatar(record: Player): ReactNode {
        if (record.ImagePath) {
            return <Avatar className="players-data__avatar" src={record.ImagePath} />;
        }
        return <Avatar icon="user" />;
    }

    private onRowClick(record: Player): void {
        const search = utils.getUrlSearch({ PlayerId: record.Id }, this.props.router.location.search);
        history.push({
            search
        });
    }

    private cellWrapper(id: string, content: ReactNode, value?: number): ReactNode {
        const search = qs.parse(this.props.router.location.search);
        const { PlayerId } = search;
        const isSelectedClass = id === PlayerId ? 'is-selected' : '';
        var textColorClass = "";

        if (value != undefined) {
            textColorClass = value < 0 ? 'red-text' : 'green-text';
        }

        return <div className={`players-data__cell-inner ${isSelectedClass} ${textColorClass}`}>{content}</div>;
    }

    onFormSubmit = (params: DateValues): void => {
        this.fetchPlayers(this.props.playersDataUrl, params);

        const search = utils.getUrlSearch(params, this.props.router.location.search);
        history.push({
            search
        });
    };

    onCheckboxesChange = (selectedColumns: string[]): void => {
        const visibleColumns = [...PERMANENT_COLUMNS, ...selectedColumns];
        this.setState({ visibleColumns });
    };

    private getCellClassName(dataIndex: string): string {
        if (this.state.visibleColumns.includes(dataIndex)) {
            return CELL_CSS_CLASS;
        }
        return `${CELL_CSS_CLASS} ${HIDDEN_CELL_CSS_CLASS}`;
    }

    private getColumns = (): ColumnProps<Player>[] => [
        {
            dataIndex: COLUMN_NAMES.ImagePath.dataIndex,
            render: (_link: any, record: Player) => {
                const content = this.getAvatar(record);
                return this.cellWrapper(record.Id, content);
            },
            width: '5%',
            className: this.getCellClassName(COLUMN_NAMES.ImagePath.dataIndex)
        },
        {
            dataIndex: COLUMN_NAMES.Name.dataIndex,
            title: COLUMN_NAMES.Name.readableName,
            className: this.getCellClassName(COLUMN_NAMES.Name.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.Id, record.Name),
            sorter: (a: Player, b: Player) => a.Name.localeCompare(b.Name)
        },
        {
            dataIndex: COLUMN_NAMES.Points.dataIndex,
            title: COLUMN_NAMES.Points.readableName,
            className: this.getCellClassName(COLUMN_NAMES.Points.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.Id, record.Points),
            sorter: (a: Player, b: Player) => b.Points - a.Points
        }, //points
        {
            dataIndex: COLUMN_NAMES.Kad.dataIndex,
            title: () => <Tooltip title="Kills / Assists / Deaths">{COLUMN_NAMES.Kad.readableName}</Tooltip>,
            className: this.getCellClassName(COLUMN_NAMES.Kad.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.Id, record.Kad),
            sorter: (a: Player, b: Player) => a.Kills - b.Kills
        }, //kad
        {
            dataIndex: COLUMN_NAMES.KdRatio.dataIndex,
            title: () => <Tooltip title="Kills / Deaths">{COLUMN_NAMES.KdRatio.readableName}</Tooltip>,
            className: this.getCellClassName(COLUMN_NAMES.KdRatio.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.Id, record.KdRatio),
            sorter: (a: Player, b: Player) => b.KdRatio - a.KdRatio
        }, //Kd ratio        
        {
        dataIndex: COLUMN_NAMES.KdDif.dataIndex,
        title: COLUMN_NAMES.KdDif.readableName,
        className: this.getCellClassName(COLUMN_NAMES.KdDif.dataIndex),
        render: (_link: any, record: Player) => this.cellWrapper(record.Id, record.KdDif, record.KdDif),
        sorter: (a: Player, b: Player) => b.KdDif - a.KdDif
        }, //kd dif
        {
            dataIndex: COLUMN_NAMES.HeadShot.dataIndex,
            title: COLUMN_NAMES.HeadShot.readableName,
            className: this.getCellClassName(COLUMN_NAMES.HeadShot.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.Id, utils.getHeadshotsString(record.HeadShot, record.Kills)),
            sorter: (a: Player, b: Player) => utils.getHeadshotsPercent(b.HeadShot, b.Kills) - utils.getHeadshotsPercent(a.HeadShot, a.Kills)
        }, //headshots
        {
            dataIndex: COLUMN_NAMES.TotalGames.dataIndex,
            title: COLUMN_NAMES.TotalGames.readableName,
            className: this.getCellClassName(COLUMN_NAMES.TotalGames.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.Id, record.TotalGames),
            sorter: (a: Player, b: Player) => b.TotalGames - a.TotalGames
        }, //total games
        {
            dataIndex: COLUMN_NAMES.Kills.dataIndex,
            title: COLUMN_NAMES.Kills.readableName,
            className: this.getCellClassName(COLUMN_NAMES.Kills.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.Id, record.Kills),
            sorter: (a: Player, b: Player) => b.Kills - a.Kills
        }, //kills
        {
            dataIndex: COLUMN_NAMES.Assists.dataIndex,
            title: COLUMN_NAMES.Assists.dataIndex,
            className: this.getCellClassName(COLUMN_NAMES.Assists.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.Id, record.Assists),
            sorter: (a: Player, b: Player) => b.Assists - a.Assists
        }, //assists
        {
            dataIndex: COLUMN_NAMES.Deaths.dataIndex,
            title: COLUMN_NAMES.Deaths.readableName,
            className: this.getCellClassName(COLUMN_NAMES.Deaths.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.Id, record.Deaths),
            sorter: (a: Player, b: Player) => b.Deaths - a.Deaths
        }, //deaths
        {
            dataIndex: COLUMN_NAMES.KdDif.dataIndex,
            title: COLUMN_NAMES.KdDif.readableName,
            className: this.getCellClassName(COLUMN_NAMES.KdDif.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.Id, record.KdDif),
            sorter: (a: Player, b: Player) => b.KdDif - a.KdDif
        },
        {
            dataIndex: COLUMN_NAMES.KillsPerGame.dataIndex,
            title: COLUMN_NAMES.KillsPerGame.readableName,
            className: this.getCellClassName(COLUMN_NAMES.KillsPerGame.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.Id, record.KillsPerGame),
            sorter: (a: Player, b: Player) => b.KillsPerGame - a.KillsPerGame
        }, //kills per game
        {
            dataIndex: COLUMN_NAMES.AssistsPerGame.dataIndex,
            title: COLUMN_NAMES.AssistsPerGame.readableName,
            className: this.getCellClassName(COLUMN_NAMES.AssistsPerGame.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.Id, record.AssistsPerGame),
            sorter: (a: Player, b: Player) => b.AssistsPerGame - a.AssistsPerGame
        }, //assists per game
        {
            dataIndex: COLUMN_NAMES.DefusedBombs.dataIndex,
            title: COLUMN_NAMES.DefusedBombs.readableName,
            className: this.getCellClassName(COLUMN_NAMES.DefusedBombs.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.Id, record.DefusedBombs),
            sorter: (a: Player, b: Player) => b.DefusedBombs - a.DefusedBombs
        }, //defused bombs
        {
            dataIndex: COLUMN_NAMES.ExplodedBombs.dataIndex,
            title: COLUMN_NAMES.ExplodedBombs.readableName,
            className: this.getCellClassName(COLUMN_NAMES.ExplodedBombs.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.Id, record.ExplodedBombs),
            sorter: (a: Player, b: Player) => b.ExplodedBombs - a.ExplodedBombs
        }, //exploded bombs
        {
            dataIndex: COLUMN_NAMES.FriendlyKills.dataIndex,
            title: COLUMN_NAMES.FriendlyKills.readableName,
            className: this.getCellClassName(COLUMN_NAMES.FriendlyKills.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.Id, record.FriendlyKills),
            sorter: (a: Player, b: Player) => b.FriendlyKills - a.FriendlyKills
        }, //friendly kills

    ];

    get columnSelector(): ReactNode {
        const { visibleColumns } = this.state;
        const colsToRender = visibleColumns.filter((x) => !PERMANENT_COLUMNS.includes(x));
        return <ColumnsSelector visibleColumns={colsToRender} onCheckboxesChange={this.onCheckboxesChange} />;
    }

    componentDidMount() {
        const search: any = qs.parse(this.props.router.location.search);
        this.fetchPlayers(this.props.playersDataUrl, search);
    }

    render(): ReactNode {
        const {
            IsLoading, Players
        } = this.props;

        const search: any = qs.parse(this.props.router.location.search);
        const { dateFrom, dateTo } = search;

        return (
            <>
                <Divider orientation="left">Choose Dates to Filter Statistics</Divider>
                <div className="players-data__filters">
                    <FilterForm
                        onFormSubmit={this.onFormSubmit}
                        isLoading={IsLoading}
                        dateFrom={dateFrom}
                        dateTo={dateTo}
                    />
                    <Dropdown overlay={this.columnSelector} trigger={['click']}>
                        <Button className="ant-dropdown-link">
                            Select columns to render <Icon type="down" />
                        </Button>
                    </Dropdown>
                </div>
                <Divider />
                <Table
                    className="players-data"
                    rowClassName={() => 'players-data__row'}
                    columns={this.getColumns()}
                    dataSource={Players}
                    rowKey={(record) => record.Id}
                    pagination={false}
                    size="middle"
                    bordered={true}
                    scroll={{ x: true }}
                    loading={IsLoading}
                    onRow={(record) => ({
                        onClick: () => {
                            this.onRowClick(record);
                        }
                    })}
                />
            </>
        );
    }
}

type PlayersDataProps = {
    playersDataUrl: string;
    IsLoading: boolean;
    Players: Player[];
    fetchPlayers: typeof fetchPlayers;
    startRequest: typeof startRequest;
    stopRequest: typeof stopRequest;
    router: any;
};

type PlayersDataState = {
    visibleColumns: string[];
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
    KdDif: ColumnMapping;
};

export type ColumnMapping = {
    dataIndex: string;
    readableName: string;
};

const mapStateToProps = (state: RootState) => {
    const IsLoading = state.app.IsLoading;
    const Players = state.app.Players;
    const router = state.router;
    return {
        IsLoading,
        Players,
        router
    };
};

export default connect(
    mapStateToProps,
    {
        fetchPlayers,
        startRequest,
        stopRequest
    }
)(PlayersData);
