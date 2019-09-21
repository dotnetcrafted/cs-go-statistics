import React from 'react';
import { RadialChart, Hint, DiscreteColorLegend } from 'react-vis';
import {  Badge, Card } from 'antd';
import randomColor from 'randomcolor';
import MapGunNameToImageUrl from './mapping/gun-image-map';
import { IGun } from '../../../general/js/redux/types';


class GunsChart extends React.Component<GunsChartProps, GunsChartState> {
    componentWillMount() {
        const {guns} = this.props;
        this._updateState(guns);
    }
    componentWillReceiveProps(nextProps: GunsChartProps) {
        const {guns} = nextProps;
        this._updateState(guns);
    }
    
    render() {
        const { data, legendItems, hoveredChartSection, selectedImageUrl, selectedImageName, isHovered } = this.state;
        return (
            <div className="guns-chart__container">
                <DiscreteColorLegend
                    className="guns-chart__legend"
                    items={legendItems}
                    onItemMouseEnter={(item:LegendItem) => this._setStateForImage(item.id)}
                    onItemMouseLeave={() => this._resetStateForImage()}
                />
                <RadialChart
                    className='guns-chart__chart'
                    innerRadius={50}
                    radius={100}
                    getAngle={(d: any) => d.theta}
                    data={data}
                    onValueMouseOver={(v: Data) => {
                        this._setStateForImage(v.id);
                        this.setState({ hoveredChartSection: v, isHovered: true });
                    }}
                    onSeriesMouseOut={(v: Data) => {
                        this._resetStateForImage();
                        this.setState({ isHovered: false });
                    }}
                    width={200}
                    height={200}
                    padAngle={0.04}
                    animation={{damping: 9, stiffness: 300}}
                >
                    {isHovered !== null ? (
                        <Hint value={hoveredChartSection} >
                            <Badge count={hoveredChartSection.theta} overflowCount={10000}>
                                <div className='guns-chart__text'>{hoveredChartSection.label}</div>
                            </Badge>
                        </Hint>
                    ) : null}
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

    _updateState(guns: IGun[]) {
        const colors: Color[] = this._getColors(guns);
        const data: Data[] = this._getData(guns, colors);
        const legendItems: LegendItem[] = this._getLegend(guns, colors);
        this.setState({colors, data, legendItems});
    }
    _getData = (guns: IGun[], colors: Color[]) => ( guns.map((g) => ({
        theta: g.Kills,
        label: g.Name,
        id: g.Id,
        style: {
            fill: this._findColor(colors, g.Id),
            stroke: false
        }
    })))
    _getColors = (guns: IGun[]) => ( guns.map((g) => ({
        id: g.Id,
        color: randomColor({
            luminosity: 'bright',
            hue: 'random'
        })
    })))
    _getLegend = (guns: IGun[], colors: Color[]) => ( guns.map((g) => ({
        title: `${g.Name}: ${g.Kills} kills`,
        id: g.Id,
        color: this._findColor(colors, g.Id),
    })))

    _setStateForImage = (id: number) => {
        const { guns } = this.props;
        const selectedImageUrl = MapGunNameToImageUrl(this._findGunName(guns, id));
        const selectedImageName = this._findGunName(guns, id);
        this.setState({ selectedImageUrl, selectedImageName})
    }

    _resetStateForImage() {
        this.setState({ selectedImageUrl: '', selectedImageName: ''})
    }

    _findColor = (colors: Color[], id: number) => {
        const colorObj = colors.find(c => c.id === id);
        return colorObj ? colorObj.color : '';
    }

    _findGunName = (guns: IGun[], id: number) => {
        const gunObj = guns.find(g => g.Id === id);
        return gunObj ? gunObj.Name : '';
    }

}

interface GunsChartState {
    data: Data[]
    colors: Color[]
    legendItems: LegendItem[]
    isHovered: boolean
    hoveredChartSection: Data
    selectedImageUrl: string
    selectedImageName: string
}

type GunsChartProps = {
    guns: IGun[]
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