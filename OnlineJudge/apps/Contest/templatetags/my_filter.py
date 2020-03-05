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
i1 = 0


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
    return no[value % 26]


@register.filter(name='match_status')
def match_status(value, id):
    print("id......"+str(id))
    w = 0
    kk = 0
    while (kk < len(value)):
        value1 = value[kk]
        print('value1:   ' + str(value1))
        if (str(value1) >= '0' and str(value1) <= '9'):
            w += 1
            if w == id:
                return str(value1)
        else:
            pass
        kk += 1



