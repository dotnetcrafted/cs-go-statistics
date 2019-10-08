import React from 'react';
import { Form, Checkbox } from 'antd';
import { FormComponentProps } from 'antd/es/form';
import { COLUMN_NAMES } from './players-data';
class ColumnsSelector extends React.Component<IColumnsSelectorProps, ColumnsSelectorState> {
    readonly state = {
        isDisabled: false,
        defaultValues: [
            COLUMN_NAMES.Points.dataIndex,
            COLUMN_NAMES.KdRatio.dataIndex,
            COLUMN_NAMES.Kills.dataIndex,
            COLUMN_NAMES.Deaths.dataIndex,
            COLUMN_NAMES.TotalGames.dataIndex
        ]
    };

    onChange(value: any[]) {
        this.props.onCheckboxesChange(value);
    }
    render() {
        const keys: string[] = Object.keys(COLUMN_NAMES);
        const excludedKeys = ['Id', 'Name', 'ImagePath', 'Achievements', 'Guns'];
        const filteredKeys = keys.filter(k => !excludedKeys.includes(k));
        const options = filteredKeys.reduce<Key[]>((previousValue: Key[], currentValue: string): Key[] => {
            previousValue.push({
                value: COLUMN_NAMES[currentValue].dataIndex,
                label: COLUMN_NAMES[currentValue].readableName
            });
            return previousValue;
        }, []);
        return (
            <Checkbox.Group
                options={options}
                defaultValue={this.state.defaultValues}
                onChange={this.onChange}
                className="columns-selector"
            />
        );
    }
}

interface IColumnsSelectorProps extends FormComponentProps {
    onCheckboxesChange: (value: any) => void;
}

type ColumnsSelectorState = {
    isDisabled: boolean;
    defaultValues: string[];
};

type Key = {
    value: string;
    label: string;
};
const WrappedFilterForm = Form.create<IColumnsSelectorProps>({ name: 'columns_selector' })(ColumnsSelector);

export default WrappedFilterForm;
