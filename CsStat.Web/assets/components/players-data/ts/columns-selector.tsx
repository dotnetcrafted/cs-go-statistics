import React from 'react';
import { Form, Checkbox, Menu } from 'antd';
import { FormComponentProps } from 'antd/es/form';
import { COLUMN_NAMES } from './players-data';

// const MAXIMUM_COLUMNS_TO_SHOW = 3;

class ColumnsSelector extends React.Component<IColumnsSelectorProps, ColumnsSelectorState> {
    readonly state = {
        isDisabled: false
    };

    private onChange = (value: any[]) => {
        this.props.onCheckboxesChange(value);
    };

    render() {
        const { visibleColumns } = this.props;
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
            <Checkbox.Group onChange={this.onChange} defaultValue={visibleColumns} className="columns-selector">
                <Menu>
                    {options &&
                        options.map(tick => (
                            <Menu.Item key={tick.value}>
                                <Checkbox value={tick.value} defaultChecked={visibleColumns.includes(tick.value)}>
                                    {tick.label}
                                </Checkbox>
                            </Menu.Item>
                        ))}
                </Menu>
            </Checkbox.Group>
        );
    }
}

interface IColumnsSelectorProps extends FormComponentProps {
    onCheckboxesChange: (value: any) => void;
    visibleColumns: string[];
}

type ColumnsSelectorState = {
    isDisabled: boolean;
};

type Key = {
    value: string;
    label: string;
};
const WrappedFilterForm = Form.create<IColumnsSelectorProps>({ name: 'columns_selector' })(ColumnsSelector);

export default WrappedFilterForm;
