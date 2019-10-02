import React from 'react';
import { Form, DatePicker, Button } from 'antd';
import { FormComponentProps } from 'antd/es/form';
import moment, { Moment } from 'moment';

const SERVER_DATE_FORMAT = 'MM/DD/YYYY';
const USER_DATE_FORMAT = 'll';

class FilterForm extends React.Component<IFilterFormProps, FilterFormState> {
    readonly state = {
        dateFrom: moment(),
        dateTo: moment(),
        dateToIsOpen: false
    };

    private handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const dateFrom = this.props.form.getFieldValue('date-picker-from');
        const dateTo = this.props.form.getFieldValue('date-picker-to');
        if (dateFrom && dateTo) {
            const values: DateValues = {
                dateFrom: dateFrom.format(SERVER_DATE_FORMAT),
                dateTo: dateTo.format(SERVER_DATE_FORMAT)
            };

            this.props.onFormSubmit(values);
        }
    };

    render() {
        const { dateFrom, dateTo } = this.props;
        const { getFieldDecorator } = this.props.form;

        return (
            <Form layout="inline" onSubmit={this.handleSubmit}>
                <Form.Item>
                    {getFieldDecorator('date-picker-from', this.getConfig(dateFrom))(
                        <DatePicker
                            format={USER_DATE_FORMAT}
                            disabledDate={this.disabledDateFrom}
                            onChange={this.onFromChange}
                            onOpenChange={this.handleFromOpenChange}
                            placeholder="From"
                        />
                    )}
                </Form.Item>
                <Form.Item>
                    {getFieldDecorator('date-picker-to', this.getConfig(dateTo))(
                        <DatePicker
                            format={USER_DATE_FORMAT}
                            disabledDate={this.disabledDateTo}
                            onChange={this.onToChange}
                            open={this.state.dateToIsOpen}
                            onOpenChange={this.handleToOpenChange}
                            placeholder="To"
                        />
                    )}
                </Form.Item>
                <Form.Item>
                    <Button type="primary" htmlType="submit" disabled={this.props.isLoading}>
                        Filter
                    </Button>
                </Form.Item>
            </Form>
        );
    }

    private getDateObject = (date: string): Moment => {
        if (date) {
            return moment(new Date(date));
        } else {
            return moment();
        }
    };

    private getConfig = (dateString: string): Config => {
        const dateObject = this.getDateObject(dateString);
        return {
            rules: [
                {
                    type: 'date',
                    required: true,
                    message: 'Please select date.'
                }
            ],
            initialValue: moment(dateObject, SERVER_DATE_FORMAT)
        };
    };

    private disabledDateFrom = (dateFrom: Moment | undefined): boolean => {
        const { dateTo } = this.state;
        if (dateFrom && dateTo) {
            return dateFrom.valueOf() >= dateTo.valueOf();
        }
        return false;
    };
    private disabledDateTo = (dateTo: Moment | undefined): boolean => {
        const { dateFrom } = this.state;
        if (dateFrom && dateTo) {
            return dateTo.valueOf() <= dateFrom.valueOf() || dateTo.valueOf() > moment().valueOf();
        }
        return false;
    };

    private onFromChange = (value: Moment | null): void => {
        if (value) {
            this.setState({
                dateFrom: value
            });
        }
    };

    private onToChange = (value: Moment | null): void => {
        if (value) {
            this.setState({
                dateTo: value
            });
        }
    };

    private handleFromOpenChange = (open: boolean): void => {
        if (!open) {
            this.setState({ dateToIsOpen: true });
        }
    };
    private handleToOpenChange = (open: boolean): void => {
        this.setState({ dateToIsOpen: open });
    };
}

export type DateValues = {
    [index: string]: string;
    dateFrom: string;
    dateTo: string;
};
interface IFilterFormProps extends FormComponentProps {
    onFormSubmit: (message: DateValues) => void;
    dateFrom: string;
    dateTo: string;
    isLoading: boolean;
}

type FilterFormState = {
    dateFrom?: Moment | null;
    dateTo?: Moment | null;
    dateToIsOpen: boolean;
};

type Config = {
    rules: [
        {
            type: string;
            required: boolean;
            message: string;
        }
    ];
    initialValue: Moment;
};

const WrappedFilterForm = Form.create<IFilterFormProps>({ name: 'filter_form' })(FilterForm);

export default WrappedFilterForm;
