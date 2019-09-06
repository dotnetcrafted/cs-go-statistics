import React from 'react';
import PropTypes from 'prop-types';
import { RadialChart, Hint, DiscreteColorLegend } from 'react-vis';
import { Typography, Badge, Card } from 'antd';
import randomColor from 'randomcolor';
import MapGunNameToImageUrl from './gun-image-map';

const { Text  } = Typography;
export default class GunsChart extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            data: [],
            colors: [],
            legendItems: [],
            hoveredChartSection: false,
            selectedImageUrl: '',
            selectedImageName: ''
        };
    }

    componentWillMount() {
        const {guns} = this.props;
        this._updateState(guns);
    }
    componentWillReceiveProps(nextProps) {
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
                    getAngle={(d) => d.theta}
                    data={data}
                    onValueMouseOver={(v) => {
                        this._setStateForImage(v.id);
                        this.setState({ hoveredChartSection: v });
                    }}
                    onSeriesMouseOut={(v) => {
                        this._resetStateForImage();
                        this.setState({ hoveredChartSection: false });
                    }}
                    width={200}
                    height={200}
                    padAngle={0.04}
                    animation={{damping: 9, stiffness: 300}}
                >
                    {hoveredChartSection !== false &&
                        <Hint value={hoveredChartSection} >
                            <Badge count={hoveredChartSection.theta}>
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

    _updateState(guns) {
        const colors = this._getColors(guns);
        const data = this._getData(guns, colors);
        const legendItems = this._getLegend(guns, colors);
        this.setState({colors, data, legendItems});
    }
    _getData = (guns, colors) => ( guns.map((g) => ({
        theta: g.Kills,
        label: g.Name,
        id: g.Id,
        style: {
            fill: colors.find(c => c.id === g.Id).color,
            stroke:false
        }
    })))
    _getColors = (guns) => ( guns.map((g) => ({
        id: g.Id,
        color: randomColor({
            luminosity: 'bright',
            hue: 'random'
        })
    })))
    _getLegend = (guns, colors) => ( guns.map((g) => ({
        title: `${g.Name}: ${g.Kills} kills`,
        id: g.Id,
        color: colors.find(c => c.id === g.Id).color
    })))

    _setStateForImage = (id) => {
        const { guns } = this.props;
        const selectedImageUrl = MapGunNameToImageUrl(guns.find(g => g.Id === id).Name);
        const selectedImageName = guns.find(g => g.Id === id).Name;
        this.setState({ selectedImageUrl, selectedImageName})
    }

    _resetStateForImage() {
        this.setState({ selectedImageUrl: '', selectedImageName: ''})
    }

}
GunsChart.propTypes = {
    guns: PropTypes.array.isRequired,
};
