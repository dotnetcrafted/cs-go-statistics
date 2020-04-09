import 'react-vis/dist/style.css';
import React from 'react';
import { RadialChart, Hint, DiscreteColorLegend } from 'react-vis';
import { Badge, Card } from 'antd';
import randomColor from 'randomcolor';
import { Gun } from 'general/ts/redux/types';
import { getWeaponById } from 'project/helpers';

class GunsChart extends React.Component<GunsChartProps, GunsChartState> {
    componentWillMount() {
        this.setState({ isHovered: false });
        const { guns } = this.props;
        this.updateState(guns);
    }

    componentWillReceiveProps(nextProps: GunsChartProps) {
        const { guns } = nextProps;
        this.updateState(guns);
    }

    updateState(guns: Gun[]) {
        const colors: Color[] = this.getColors(guns);
        const data: Data[] = this.getData(guns, colors);
        const legendItems: LegendItem[] = this.getLegend(guns, colors);
        this.setState({ colors, data, legendItems });
    }

    getData = (guns: Gun[], colors: Color[]) =>
        guns.map((g) => ({
            theta: g.kills,
            label: g.name,
            id: g.id,
            style: {
                fill: this.findColor(colors, g.id),
                stroke: false
            }
        }));

    getColors = (guns: Gun[]) =>
        guns.map((g) => ({
            id: g.id,
            color: randomColor({
                luminosity: 'bright',
                hue: 'random'
            })
        }));

    getLegend = (guns: Gun[], colors: Color[]) =>
        guns.map((g) => ({
            title: `${g.name}: ${g.kills} kills`,
            id: g.id,
            color: this.findColor(colors, g.id)
        }));

    setStateForImage = (id: number) => {
        const weapon = getWeaponById(id);

        if (!weapon) {
            this.resetStateForImage();
            return;
        }

        this.setState({
            selectedImageUrl: weapon.photoImage,
            selectedImageName: weapon.name
        });
    };

    resetStateForImage() {
        this.setState({ selectedImageUrl: '', selectedImageName: '' });
    }

    findColor = (colors: Color[], id: number) => {
        const colorObj = colors.find((c) => c.id === id);
        return colorObj ? colorObj.color : '';
    };

    render() {
        const {
            data, legendItems, hoveredChartSection, selectedImageUrl, selectedImageName, isHovered
        } = this.state;

        return (
            <div className="guns-chart__container">
                <DiscreteColorLegend
                    className="guns-chart__legend"
                    items={legendItems}
                    onItemMouseEnter={(item: LegendItem) => this.setStateForImage(item.id)}
                    onItemMouseLeave={() => this.resetStateForImage()}
                />
                <RadialChart
                    className="guns-chart__chart"
                    innerRadius={50}
                    radius={100}
                    getAngle={(d: Data) => d.theta}
                    data={data}
                    onValueMouseOver={(v: Data) => {
                        this.setStateForImage(v.id);
                        this.setState({ hoveredChartSection: v, isHovered: true });
                    }}
                    onSeriesMouseOut={() => {
                        this.resetStateForImage();
                        this.setState({ isHovered: false });
                    }}
                    width={200}
                    height={200}
                    padAngle={0.04}
                    animation={{ damping: 9, stiffness: 300 }}
                >
                    {isHovered !== false ? (
                        <Hint value={hoveredChartSection}>
                            <Badge count={hoveredChartSection.theta} overflowCount={10000}>
                                <div className="guns-chart__text">{hoveredChartSection.label}</div>
                            </Badge>
                        </Hint>
                    ) : null}
                </RadialChart>
                <div className="guns-chart__card-wrapper">
                    {selectedImageUrl && (
                        <Card
                            className="guns-chart__card"
                            size="small"
                            title={selectedImageName}
                            bodyStyle={{ position: 'relative', flex: '1' }}
                        >
                            <div className="guns-chart__img-wrapper">
                                <img src={selectedImageUrl}></img>
                            </div>
                        </Card>
                    )}
                </div>
            </div>
        );
    }
}

type GunsChartState = {
    data: Data[];
    colors: Color[];
    legendItems: LegendItem[];
    isHovered: boolean;
    hoveredChartSection: Data;
    selectedImageUrl: string;
    selectedImageName: string;
};

type GunsChartProps = {
    guns: Gun[];
};

type Color = {
    id: number;
    color: any;
};
type Data = {
    theta: number;
    label: string;
    id: number;
    style: {
        fill: string;
        stroke: boolean;
    };
};
type LegendItem = {
    id: number;
    title: string;
    color: string;
};
export default GunsChart;
