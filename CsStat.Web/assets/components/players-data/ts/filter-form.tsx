import React from 'react';
import {
    Form, DatePicker, Radio, Button, Row, Col
} from 'antd';
import { FormComponentProps } from 'antd/es/form';
import moment, { Moment } from 'moment';
import en_GB from 'antd/es/date-picker/locale/en_GB';

const SERVER_DATE_FORMAT = 'MM/DD/YYYY';
const USER_DATE_FORMAT = 'll';

const RadioGroup = Radio.Group;

const pariodsDay = [
    { label: 'All day', value: 'All' },
    { label: 'Afternoon', value: 'Afternoon' },
    { label: 'Evening', value: 'Evening' },
];

class FilterForm extends React.Component<IFilterFormProps, FilterFormState> {
    readonly state = {
        dateFrom: moment(),
        dateTo: moment(),
        dateToIsOpen: false,
        periodDay: 'All',
    };
    
    render() {
        const { dateFrom, dateTo } = this.props;
        const { getFieldDecorator } = this.props.form;

        return (
            <Form layout="inline" onSubmit={this.handleSubmit} className="filter-form">
                <Row>
                    <Col span={24}>
                        <Form.Item>
                            <RadioGroup
                                options={pariodsDay}
                                value={this.state.periodDay}
                                onChange={this.onPeriodDayChange}
                            />
                        </Form.Item>
                    </Col>
                </Row>
                <Row>
                    <Col span={24}>
                        <Form.Item>
                            {getFieldDecorator('date-picker-from', this.getConfig(dateFrom))(
                                <DatePicker
                                    format={USER_DATE_FORMAT}
                                    disabledDate={this.disabledDateFrom}
                                    onChange={this.onFromChange}
                                    onOpenChange={this.handleFromOpenChange}
                                    placeholder="From"
                                    locale={en_GB}
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
                                    locale={en_GB}
                                />
                            )}
                        </Form.Item>
                        <Form.Item>
                            <Button type="primary" htmlType="submit" disabled={this.props.isLoading}>
                                Filter
                            </Button>
                        </Form.Item>
                    </Col>
                </Row>
                <Row>
                    <Col span={24}>
                        <Form.Item>
                            <Button onClick={this.onDayButtonClick} disabled={this.props.isLoading}>
                                Today
                            </Button>
                        </Form.Item>
                        <Form.Item>
                            <Button onClick={this.onWeekButtonClick} disabled={this.props.isLoading}>
                                This Week
                            </Button>
                        </Form.Item>
                        <Form.Item>
                            <Button onClick={this.onMonthButtonClick} disabled={this.props.isLoading}>
                                This Month
                            </Button>
                        </Form.Item>
                        <Form.Item>
                            <Button onClick={this.onAllButtonClick} disabled={this.props.isLoading}>
                                All Time
                            </Button>
                        </Form.Item>
                    </Col>
                </Row>
            </Form>
        );
    }

    private onPeriodDayChange = (e: any) => {
        this.setState({
            periodDay: e.target.value            
        });
    };

    private getDateObject = (date?: string): Moment => {
        if (date) {
            return moment(new Date(date));
        }
        return moment();
    };

    private getConfig = (dateString?: string): Config => {
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

        if (moment(dateFrom).isAfter(moment())) {
            return true;
        } if (moment(dateFrom).isSameOrAfter(dateTo, 'day')) {
            return false;
        }
        return false;
    };

    private disabledDateTo = (dateTo: Moment | undefined): boolean => {
        const { dateFrom } = this.state;
        if (moment(dateTo).isAfter(moment())) {
            return true;
        } if (moment(dateTo).isSameOrAfter(dateFrom, 'day')) {
            return false;
        }
        return true;
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

    private handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const dateFrom = this.props.form.getFieldValue('date-picker-from');
        const dateTo = this.props.form.getFieldValue('date-picker-to');
        if (dateFrom && dateTo) {
            const values: DateValues = {
                dateFrom: dateFrom.format(SERVER_DATE_FORMAT),
                dateTo: dateTo.format(SERVER_DATE_FORMAT),
                periodDay: this.state.periodDay
            };

            this.props.onFormSubmit(values);
        }
    };

    private onDayButtonClick = () => {
        const values: DateValues = {
            dateFrom: moment().format(SERVER_DATE_FORMAT),
            dateTo: moment().format(SERVER_DATE_FORMAT),
            periodDay: this.state.periodDay
        };

        this.props.onFormSubmit(values);
    };

    private onWeekButtonClick = () => {
        const values: DateValues = {
            dateFrom: moment()
                .startOf('isoWeek')
                .format(SERVER_DATE_FORMAT),
            dateTo: moment().format(SERVER_DATE_FORMAT),
            periodDay: this.state.periodDay
        };

        this.props.onFormSubmit(values);
    };

    private onMonthButtonClick = () => {
        const values: DateValues = {
            dateFrom: moment()
                .startOf('month')
                .format(SERVER_DATE_FORMAT),
            dateTo: moment().format(SERVER_DATE_FORMAT),
            periodDay: this.state.periodDay
        };

        this.props.onFormSubmit(values);
    };

    private onAllButtonClick = () => {
        const values: DateValues = {
            dateFrom: '',
            dateTo: moment().format(SERVER_DATE_FORMAT),
            periodDay: this.state.periodDay
        };

        this.props.onFormSubmit(values);
    };
}

export type DateValues = {
    [index: string]: string;
    dateFrom: string;
    dateTo: string;
    periodDay: string;
};
interface IFilterFormProps extends FormComponentProps {
    onFormSubmit: (message: DateValues) => void;
    dateFrom?: string;
    dateTo?: string;
    isLoading: boolean;
    periodDay?: string;
}

type FilterFormState = {
    dateFrom?: Moment | null;
    dateTo?: Moment | null;
    dateToIsOpen: boolean;
    periodDay?: string;
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
