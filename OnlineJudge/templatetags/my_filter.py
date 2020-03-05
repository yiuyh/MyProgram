from django import template
register = template.Library()


no = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q',
      'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z']
i = 0
j = 0
j1 = 0
j2 = 0
k = 0
x = 0
y = 0


@register.filter(name='plratio')
def plratio(value):
    global y
    if y < len(value):
        value1 = value[y]
        y += 1
        if y == len(value):
            y = 0
        return str(value1)
    else:
        return ''


@register.filter(name='my_filter')
def my_filter(value):
    global i
    if i < len(value):
        value1 = value[i]
        i += 1
        if i == len(value):
            i = 0
        return str(value1)
    else:
        return ''


@register.filter(name='solved')
def solved(value):
    global j
    if j < len(value):
        value1 = value[j]
        j += 1
        if j == len(value):
            j = 0
        return str(value1)
    else:
        return ''

@register.filter(name='solved1')
def solved1(value):
    global j1
    if j1 < len(value):
        value1 = value[j1]
        j1 += 1
        if j1 == len(value):
            j1 = 0
        return str(value1)
    else:
        return ''

@register.filter(name='solved2')
def solved2(value):
    global j2
    if j2 < len(value):
        value1 = value[j2]
        j2 += 1
        if j2 == len(value):
            j2 = 0
        return str(value1)
    else:
        return ''

contest_ratio_list_filter_var = 0
@register.filter(name='contest_ratio_list_filter')
def contest_ratio_list_filter(value):
    global contest_ratio_list_filter_var
    if contest_ratio_list_filter_var < len(value):
        value1 = value[contest_ratio_list_filter_var]
        contest_ratio_list_filter_var += 1
        if contest_ratio_list_filter_var == len(value):
            contest_ratio_list_filter_var = 0
        return str(value1)
    else:
        return ''

contest_ac_nums_filter_var = 0
@register.filter(name='contest_ac_nums_filter')
def contest_ac_nums_filter(value):
    global contest_ac_nums_filter_var
    if contest_ac_nums_filter_var < len(value):
        value1 = value[j]
        contest_ac_nums_filter_var += 1
        if contest_ac_nums_filter_var == len(value):
            contest_ac_nums_filter_var = 0
        return str(value1)
    else:
        return ''

contest_submit_nums_filter_var = 0
@register.filter(name='contest_submit_nums_filter')
def contest_submit_nums_filter(value):
    global contest_submit_nums_filter_var
    if contest_submit_nums_filter_var < len(value):
        value1 = value[j]
        contest_submit_nums_filter_var += 1
        if contest_submit_nums_filter_var == len(value):
            contest_submit_nums_filter_var = 0
        return str(value1)
    else:
        return ''


@register.filter(name='no_to_char')
def no_to_char(value):
    global k
    c = None
    if k < len(value) and k < 26:
        c = no[k]
    elif k < len(value) and k < 51:
        c = no[k].lower()
    elif k < len(value):
        c = str(value[k])
    k += 1
    if k == len(value):
        k = 0
    return c


@register.filter(name='rank_char')
def rank_char(value):
    global x
    c = None
    if len(value) < 26:
        c = no[x]
    elif len(value) < 51:
        c = no[x-26].lower()
    elif 51 <= len(value):
        c = str(value[x])
    x += 1
    if x == len(value):
        x = 0
    return c


