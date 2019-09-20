import React from 'react';
import { RadialChart, Hint, DiscreteColorLegend } from 'react-vis';
import {  Badge, Card } from 'antd';
import randomColor from 'randomcolor';
import MapGunNameToImageUrl from './mapping/gun-image-map';
import { IGuns } from '../../../general/js/redux/types';


class GunsChart extends React.Component<GunsChartProps, GunsChartState> {
    readonly state = {
        data: [],
        colors: [],
        legendItems: [],
        hoveredChartSection: null,
        selectedImageUrl: '',
        selectedImageName: ''
    };

    componentWillMount() {
        const {guns} = this.props;
        this._updateState(guns);
    }
    componentWillReceiveProps(nextProps: GunsChartProps) {
        const {guns} = nextProps;
        this._updateState(guns);
    }
    
    render() {
        const {guns} = this.props;
        const { data, legendItems, hoveredChartSection, selectedImageUrl, selectedImageName } = this.state;
        return (
            <div className="guns-chart__container">
                <DiscreteColorLegend
                    className="guns-chart__legend"
                    items={legendItems}
                    onItemMouseEnter={(item) => this._setStateForImage(item.id)}
                    onItemMouseLeave={() => this._resetStateForImage()}
                />
                <RadialChart
                    className='guns-chart__chart'
                    innerRadius={50}
                    radius={100}
                    getAngle={(d: any) => d.theta}
                    data={data}
                    onValueMouseOver={(v: any) => {
                        this._setStateForImage(v.id);
                        this.setState({ hoveredChartSection: v });
                    }}
                    onSeriesMouseOut={(v) => {
                        this._resetStateForImage();
                        this.setState({ hoveredChartSection: null });
                    }}
                    width={200}
                    height={200}
                    padAngle={0.04}
                    animation={{damping: 9, stiffness: 300}}
                >
                    {hoveredChartSection !== null &&
                        <Hint value={hoveredChartSection} >
                            <Badge count={hoveredChartSection.theta} overflowCount={10000}>
                                <div className='guns-chart__text'>{hoveredChartSection.label}</div>
                            </Badge>
                        </Hint>
                    }
                </RadialChart>
                <div className="guns-chart__card-wrapper">
                    {selectedImageUrl &&
                        <Card
                            className="guns-chart__card"
                            size="small"
                            title={selectedImageName}
                            bodyStyle={{position: 'relative', flex: '1'}}
                        >
                            <div className="guns-chart__img-wrapper">
                                <img src={selectedImageUrl}></img>
                            </div>
                        </Card>
                    }
                </div>
            </div>
        );
    }

    _updateState(guns: IGuns[]) {
        const colors: Color[] = this._getColors(guns);
        const data: Data[] = this._getData(guns, colors);
        const legendItems: LegendItem[] = this._getLegend(guns, colors);
        this.setState({colors, data, legendItems});
    }
    _getData = (guns: IGuns[], colors: Color[]) => ( guns.map((g) => ({
        theta: g.Kills,
        label: g.Name,
        id: g.Id,
        style: {
            fill: colors.find(c => c.id === g.Id).color,
            stroke:false
        }
    })))
    _getColors = (guns: IGuns[]) => ( guns.map((g) => ({
        id: g.Id,
        color: randomColor({
            luminosity: 'bright',
            hue: 'random'
        })
    })))
    _getLegend = (guns: IGuns[], colors: Color[]) => ( guns.map((g) => ({
        title: `${g.Name}: ${g.Kills} kills`,
        id: g.Id,
        color: colors.find(c => c.id === g.Id).color
    })))

    _setStateForImage = (id: string) => {
        const { guns } = this.props;
        const selectedImageUrl = MapGunNameToImageUrl(guns.find(g => g.Id === id).Name);
        const selectedImageName = guns.find(g => g.Id === id).Name;
        this.setState({ selectedImageUrl, selectedImageName})
    }

    _resetStateForImage() {
        this.setState({ selectedImageUrl: '', selectedImageName: ''})
    }

}

interface GunsChartState {
    data: Data[]
    colors: Color[]
    legendItems: LegendItem[]
    hoveredChartSection?: any
    selectedImageUrl: string
    selectedImageName: string
}

type GunsChartProps = {
    guns: IGuns[]
}

type Color = {
    id: number
    color: any
}
type Data = {
    theta: number
    label: string
    id: number
    style: {
        fill: string
        stroke: boolean
    }
}
type LegendItem = {
    id: number
    title: string
    color: string
}
export default GunsChart;