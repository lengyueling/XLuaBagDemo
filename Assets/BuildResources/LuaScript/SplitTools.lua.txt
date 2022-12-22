function string.split(input, delimiter)
    input = tostring(input)
    delimiter = tostring(delimiter)
    if (delimiter=='') then
        return false
    end
    local pos,arr = 0, {}

    local find = function() 
        return string.find(input, delimiter, pos, true) 
    end

    for st,sp in find do
        table.insert(arr, string.sub(input, pos, st - 1))
        pos = sp + 1
    end
    table.insert(arr, string.sub(input, pos))
    return arr
end








