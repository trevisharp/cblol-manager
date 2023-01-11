function count()
{
    $files = ls

    $total = 0;

    for ($i = 0; $i -le $files.Length; $i += 1)
    {
        if ($files[$i].Extension -eq ".cs")
        {
            $total += (Get-content $files[$i]).Length
        }
        if ($files[$i].Extension -eq "")
        {
            cd $files[$i]
            $total += count
            cd ..
        }
    }

    return $total
}

count